using AutoMapper;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Services
{
    public class SubmissionService : ISubmissionService
    {
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IUserAssessmentRepository _userAssessmentRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SubmissionService(ISubmissionRepository submissionRepository,
                                 IUserAssessmentRepository userAssessmentRepository,
                                 IQuestionRepository questionRepository,
                                 IUserRepository userRepository,
                                 IMapper mapper)
        {
            _userRepository = userRepository;
            _submissionRepository = submissionRepository;
            _userAssessmentRepository = userAssessmentRepository;
            _questionRepository = questionRepository;
            _mapper = mapper;
        }

        public async Task<SubmissionDto> CreateSubmissionAsync(SubmissionCreateDto submissionDto, int userId)
        {
            var userAssessment = await _userAssessmentRepository.GetByIdAsync(submissionDto.UserAssessmentId);
            if (userAssessment == null || userAssessment.UserId != userId)
            {
                throw new UnauthorizedAccessException("User assessment not found or not assigned to this user.");
            }
            if (userAssessment.Status != Enums.UserAssessmentStatus.InProgress)
            {
                throw new InvalidOperationException("Cannot submit answers for an assessment that is not in progress.");
            }

            var question = await _questionRepository.GetByIdAsync(submissionDto.QuestionId);
            if (question == null || question.AssessmentId != userAssessment.AssessmentId)
            {
                throw new KeyNotFoundException("Question not found or does not belong to this assessment.");
            }

            // Check if submission for this question already exists for this userAssessment
            var existingSubmission = await _submissionRepository.SingleOrDefaultAsync(
                s => s.UserAssessmentId == submissionDto.UserAssessmentId && s.QuestionId == submissionDto.QuestionId);

            Submission submission;
            if (existingSubmission != null)
            {
                // Update existing submission
                submission = existingSubmission;
                _mapper.Map(submissionDto, submission);
                submission.SubmittedAt = DateTime.Now;
                _submissionRepository.Update(submission);
            }
            else
            {
                // Create new submission
                submission = _mapper.Map<Submission>(submissionDto);
                submission.SubmittedAt = DateTime.Now;
                await _submissionRepository.AddAsync(submission);
            }

            // Re-fetch with question details for DTO mapping
            var submissionWithQuestion = await _submissionRepository.GetSubmissionWithDetailsAsync(submission.SubmissionId);
            return _mapper.Map<SubmissionDto>(submissionWithQuestion);
        }

        public async Task<IEnumerable<SubmissionDto>> GetSubmissionsByUserAssessmentIdAsync(int userAssessmentId, int userId)
        {
            var userAssessment = await _userAssessmentRepository.GetByIdAsync(userAssessmentId);
            //if (userAssessment == null || (userAssessment.UserId != userId && (await _userRepository.GetByIdAsync(userId)).Role.RoleName != "Admin" && (await _userRepository.GetByIdAsync(userId)).Role.RoleName != "Evaluator"))
            if (userAssessment == null)
            {
                throw new KeyNotFoundException($"UserAssessment with ID {userAssessmentId} not found.");
            }

            var submissions = await _submissionRepository.GetSubmissionsByUserAssessmentIdAsync(userAssessmentId);
            return _mapper.Map<IEnumerable<SubmissionDto>>(submissions);
        }

        public async Task<SubmissionDto> GetSubmissionByIdAsync(int submissionId, int userId)
        {
            var submission = await _submissionRepository.GetSubmissionWithDetailsAsync(submissionId);
            if (submission == null) return null;

            var userAssessment = await _userAssessmentRepository.GetByIdAsync(submission.UserAssessmentId);
            //if (userAssessment == null || (userAssessment.UserId != userId && (await _userRepository.GetByIdAsync(userId)).Role.RoleName != "Admin" && (await _userRepository.GetByIdAsync(userId)).Role.RoleName != "Evaluator"))
            if (userAssessment == null)
            {
                //throw new UnauthorizedAccessException("Not authorized to view this submission.");
                throw new KeyNotFoundException($"UserAssessment with submission ID {submissionId} not found.");
            }

            return _mapper.Map<SubmissionDto>(submission);
        }
    }
}
