using AutoMapper;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IMapper _mapper;

        public QuestionService(IQuestionRepository questionRepository, IAssessmentRepository assessmentRepository, IMapper mapper)
        {
            _questionRepository = questionRepository;
            _assessmentRepository = assessmentRepository;
            _mapper = mapper;
        }

        public async Task<QuestionDto> AddQuestionToAssessmentAsync(int assessmentId, QuestionCreateDto questionDto)
        {
            var assessment = await _assessmentRepository.GetByIdAsync(assessmentId);
            if (assessment == null)
            {
                throw new KeyNotFoundException($"Assessment with ID {assessmentId} not found.");
            }

            var question = _mapper.Map<Question>(questionDto);
            question.AssessmentId = assessmentId;
            await _questionRepository.AddAsync(question);
            return _mapper.Map<QuestionDto>(question);
        }

        public async Task<IEnumerable<QuestionDto>> GetQuestionsByAssessmentIdAsync(int assessmentId)
        {
            var questions = await _questionRepository.GetQuestionsByAssessmentIdAsync(assessmentId);
            return _mapper.Map<IEnumerable<QuestionDto>>(questions);
        }

        public async Task<QuestionDto> GetQuestionByIdAsync(int questionId)
        {
            var question = await _questionRepository.GetByIdAsync(questionId);
            if (question == null) return null;
            return _mapper.Map<QuestionDto>(question);
        }

        public async Task UpdateQuestionAsync(int questionId, QuestionUpdateDto questionDto)
        {
            var questionToUpdate = await _questionRepository.GetByIdAsync(questionId);
            if (questionToUpdate == null)
            {
                throw new KeyNotFoundException($"Question with ID {questionId} not found.");
            }

            _mapper.Map(questionDto, questionToUpdate);
            _questionRepository.Update(questionToUpdate);
        }

        public async Task DeleteQuestionAsync(int questionId)
        {
            var questionToDelete = await _questionRepository.GetByIdAsync(questionId);
            if (questionToDelete == null)
            {
                throw new KeyNotFoundException($"Question with ID {questionId} not found.");
            }
            _questionRepository.Delete(questionToDelete);
        }
    }

}
