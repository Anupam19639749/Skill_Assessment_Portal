using AutoMapper;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Enums;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Services
{
    public class ResultService : IResultService
    {
        private readonly IResultRepository _resultRepository;
        private readonly IUserAssessmentRepository _userAssessmentRepository;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository; // For role checks
        private readonly IMapper _mapper;

        public ResultService(IResultRepository resultRepository,
                             IUserAssessmentRepository userAssessmentRepository,
                             ISubmissionRepository submissionRepository,
                             IQuestionRepository questionRepository,
                             IUserRepository userRepository,
                             IMapper mapper)
        {
            _resultRepository = resultRepository;
            _userAssessmentRepository = userAssessmentRepository;
            _submissionRepository = submissionRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<ResultDto> GenerateResultAsync(int userAssessmentId)
        {
            var userAssessment = await _userAssessmentRepository.GetUserAssessmentWithDetailsAsync(userAssessmentId);
            if (userAssessment == null)
            {
                throw new KeyNotFoundException($"User assessment with ID {userAssessmentId} not found.");
            }
            if (userAssessment.Status != UserAssessmentStatus.Submitted)
            {
                throw new InvalidOperationException("Cannot generate result for an assessment that is not submitted.");
            }

            // Check if a result already exists to update it, otherwise create new
            var result = await _resultRepository.GetResultByUserAssessmentIdAsync(userAssessmentId);
            bool isNewResult = (result == null);
            if (isNewResult)
            {
                result = new Result { UserAssessmentId = userAssessmentId };
            }

            var submissions = await _submissionRepository.GetSubmissionsByUserAssessmentIdAsync(userAssessmentId);
            int totalMarksObtained = 0;
            int maxPossibleMarks = 0;

            foreach (var submission in submissions)
            {
                var question = await _questionRepository.GetByIdAsync(submission.QuestionId);
                if (question == null) continue;

                maxPossibleMarks += question.MaxMarks;

                // Auto-evaluate MCQ and non-descriptive types
                if (question.QuestionType == QuestionType.MCQ)
                {
                    if (submission.AnswerText?.Trim().Equals(question.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase) == true)
                    {
                        submission.IsCorrect = true;
                        submission.MarksObtained = question.MaxMarks;
                    }
                    else
                    {
                        submission.IsCorrect = false;
                        submission.MarksObtained = 0;
                    }
                    _submissionRepository.Update(submission); // Update submission with auto-eval result
                }
                // For Descriptive/Coding/FileUpload, marks are initially null and need manual evaluation
                // We sum up marks obtained so far, whether auto-evaluated or manually set
                totalMarksObtained += submission.MarksObtained ?? 0;
            }

            result.TotalMarksObtained = totalMarksObtained;
            result.Percentage = maxPossibleMarks > 0 ? (float)totalMarksObtained / maxPossibleMarks * 100 : 0;

            // Example pass/fail threshold
            result.Passed = result.Percentage >= 60; // Assuming 60% as passing score
            result.EvaluatedAt = DateTime.Now;

            if (isNewResult)
            {
                await _resultRepository.AddAsync(result);
            }
            else
            {
                _resultRepository.Update(result);
            }

            // Re-fetch result with user assessment details for DTO mapping
            var updatedResult = await _resultRepository.GetResultByUserAssessmentIdAsync(userAssessmentId);
            return _mapper.Map<ResultDto>(updatedResult);
        }

        public async Task<ResultDto> GetResultByUserAssessmentIdAsync(int userAssessmentId, int userId)
        {
            var result = await _resultRepository.GetResultByUserAssessmentIdAsync(userAssessmentId);
            if (result == null) return null;

            var userAssessment = await _userAssessmentRepository.GetByIdAsync(userAssessmentId);
            if (userAssessment == null || (userAssessment.UserId != userId && (await _userRepository.GetByIdAsync(userId)).Role.RoleName != "Admin" && (await _userRepository.GetByIdAsync(userId)).Role.RoleName != "Evaluator"))
            {
                throw new UnauthorizedAccessException("Not authorized to view this result.");
            }

            // Re-fetch result with user assessment details for DTO mapping
            var resultWithDetails = await _resultRepository.GetResultByUserAssessmentIdAsync(userAssessmentId);
            return _mapper.Map<ResultDto>(resultWithDetails);
        }

        public async Task UpdateResultManualEvaluationAsync(int submissionId, int marksObtained)
        {
            var submission = await _submissionRepository.GetSubmissionWithDetailsAsync(submissionId);
            if (submission == null)
            {
                throw new KeyNotFoundException($"Submission with ID {submissionId} not found.");
            }

            var question = submission.Question;
            if (question == null)
            {
                throw new InvalidOperationException("Submission's question not found.");
            }

            if (marksObtained < 0 || marksObtained > question.MaxMarks)
            {
                throw new ArgumentOutOfRangeException(nameof(marksObtained), $"Marks must be between 0 and {question.MaxMarks}.");
            }

            submission.MarksObtained = marksObtained;
            submission.IsCorrect = (marksObtained > 0); // Assuming any marks means it's partially correct
            _submissionRepository.Update(submission);

            // Re-generate the overall assessment result after a manual submission update
            await GenerateResultAsync(submission.UserAssessmentId);
        }
    }
}
