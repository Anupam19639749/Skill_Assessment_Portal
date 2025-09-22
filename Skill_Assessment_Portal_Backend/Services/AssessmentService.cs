using AutoMapper;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserRepository _userRepository; // To get creator details
        private readonly IMapper _mapper;

        public AssessmentService(IAssessmentRepository assessmentRepository, IUserRepository userRepository, IMapper mapper)
        {
            _assessmentRepository = assessmentRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<AssessmentDto> CreateAssessmentAsync(AssessmentCreateDto assessmentDto, int createdByUserId)
        {
            var assessment = _mapper.Map<Assessment>(assessmentDto);
            assessment.CreatedBy = createdByUserId;
            assessment.CreatedAt = DateTime.Now; // Ensure timestamp is set here or in model constructor

            await _assessmentRepository.AddAsync(assessment);

            // Re-fetch to include Creator and Questions for accurate DTO mapping (e.g., CreatorName, QuestionCount)
            var createdAssessment = await _assessmentRepository.GetAssessmentByIdWithDetailsAsync(assessment.AssessmentId);
            return _mapper.Map<AssessmentDto>(createdAssessment);
        }

        public async Task<IEnumerable<AssessmentDto>> GetAllAssessmentsAsync()
        {
            var assessments = await _assessmentRepository.GetAssessmentsWithDetailsAsync();
            return _mapper.Map<IEnumerable<AssessmentDto>>(assessments);
        }

        public async Task<AssessmentDto> GetAssessmentByIdAsync(int id)
        {
            var assessment = await _assessmentRepository.GetAssessmentByIdWithDetailsAsync(id);
            if (assessment == null) return null;
            return _mapper.Map<AssessmentDto>(assessment);
        }

        public async Task UpdateAssessmentAsync(int id, AssessmentUpdateDto assessmentDto)
        {
            var assessmentToUpdate = await _assessmentRepository.GetByIdAsync(id);
            if (assessmentToUpdate == null)
            {
                throw new KeyNotFoundException($"Assessment with ID {id} not found.");
            }

            _mapper.Map(assessmentDto, assessmentToUpdate); // Update existing entity
            _assessmentRepository.Update(assessmentToUpdate);
        }

        //public async Task DeleteAssessmentAsync(int id)
        //{
        //    var assessmentToDelete = await _assessmentRepository.GetByIdAsync(id);
        //    if (assessmentToDelete == null)
        //    {
        //        throw new KeyNotFoundException($"Assessment with ID {id} not found.");
        //    }
        //    _assessmentRepository.Delete(assessmentToDelete);
        //}
    }
}
