//This service handles assigning assessments, starting attempts, and submission
using AutoMapper;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Enums;
using Skill_Assessment_Portal_Backend.Interfaces.IRepositories;
using Skill_Assessment_Portal_Backend.Interfaces.IServices;
using Skill_Assessment_Portal_Backend.Models;

namespace Skill_Assessment_Portal_Backend.Services
{
    public class UserAssessmentService : IUserAssessmentService
    {
        private readonly IUserAssessmentRepository _userAssessmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IResultService _resultService; // To trigger result generation
        private readonly IMapper _mapper;

        public UserAssessmentService(IUserAssessmentRepository userAssessmentRepository,
                                     IUserRepository userRepository,
                                     IAssessmentRepository assessmentRepository,
                                     IResultService resultService, // Inject ResultService
                                     IMapper mapper)
        {
            _userAssessmentRepository = userAssessmentRepository;
            _userRepository = userRepository;
            _assessmentRepository = assessmentRepository;
            _resultService = resultService;
            _mapper = mapper;
        }

        public async Task AssignAssessmentToUsersAsync(UserAssessmentAssignDto assignDto)
        {
            var assessment = await _assessmentRepository.GetByIdAsync(assignDto.AssessmentId);
            if (assessment == null)
            {
                throw new KeyNotFoundException($"Assessment with ID {assignDto.AssessmentId} not found.");
            }

            foreach (var userId in assignDto.UserIds)
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    // Log or handle missing user, but don't stop the whole assignment
                    Console.WriteLine($"Warning: User with ID {userId} not found. Skipping assignment.");
                    continue;
                }

                // Check if the user is already assigned this assessment (optional, handled by unique index)
                var existingAssignment = await _userAssessmentRepository
                    .SingleOrDefaultAsync(ua => ua.UserId == userId && ua.AssessmentId == assignDto.AssessmentId);

                if (existingAssignment == null)
                {
                    var userAssessment = new UserAssessment
                    {
                        UserId = userId,
                        AssessmentId = assignDto.AssessmentId,
                        ScheduledAt = assignDto.ScheduledAt,
                        Status = UserAssessmentStatus.NotStarted
                    };
                    await _userAssessmentRepository.AddAsync(userAssessment);
                }
            }
        }
        public async Task<UserAssessmentDto> GetUserAssessmentDetailsAsync(int userAssessmentId)
        {
            var userAssessment = await _userAssessmentRepository.GetUserAssessmentWithDetailsAsync(userAssessmentId);
            if (userAssessment == null) return null;
            return _mapper.Map<UserAssessmentDto>(userAssessment);
        }

        public async Task<IEnumerable<UserAssessmentDto>> GetUserAssessmentsForAdminAsync()
        {
            var userAssessments = await _userAssessmentRepository.GetAllAsync(); // Gets all, needs details for DTO
            var userAssessmentsWithDetails = new List<UserAssessment>();
            foreach (var ua in userAssessments)
            {
                var uaWithDetails = await _userAssessmentRepository.GetUserAssessmentWithDetailsAsync(ua.UserAssessmentId);
                if (uaWithDetails != null) userAssessmentsWithDetails.Add(uaWithDetails);
            }
            return _mapper.Map<IEnumerable<UserAssessmentDto>>(userAssessmentsWithDetails);
        }


        public async Task<IEnumerable<UserAssessmentDto>> GetUserAssessmentsForUserAsync(int userId)
        {
            var userAssessments = await _userAssessmentRepository.GetUserAssessmentsByUserIdAsync(userId);
            // Need to eager load Result for DTO mapping
            var userAssessmentsWithDetails = new List<UserAssessment>();
            foreach (var ua in userAssessments)
            {
                var uaWithDetails = await _userAssessmentRepository.GetUserAssessmentWithDetailsAsync(ua.UserAssessmentId);
                if (uaWithDetails != null) userAssessmentsWithDetails.Add(uaWithDetails);
            }

            return _mapper.Map<IEnumerable<UserAssessmentDto>>(userAssessmentsWithDetails);
        }
        public async Task<IEnumerable<UserAssessmentDto>> GetUserAssessmentsByAssessmentIdAsync(int assessementId)
        {
            var userAssessments = await _userAssessmentRepository.GetUserAssessmentsByAssessmentIdAsync(assessementId);
            var userAssessmentsWithDetails = new List<UserAssessment>();
            foreach (var ua in userAssessments)
            {
                var uaWithDetails = await _userAssessmentRepository.GetUserAssessmentWithDetailsAsync(ua.UserAssessmentId);
                if (uaWithDetails != null) userAssessmentsWithDetails.Add(uaWithDetails);
            }

            return _mapper.Map<IEnumerable<UserAssessmentDto>>(userAssessmentsWithDetails);
        }

        public async Task StartAssessmentAsync(int userAssessmentId, int userId)
        {
            var userAssessment = await _userAssessmentRepository.GetByIdAsync(userAssessmentId);
            if (userAssessment == null || userAssessment.UserId != userId)
            {
                throw new UnauthorizedAccessException("Assessment not found or not assigned to this user.");
            }
            if (userAssessment.Status == UserAssessmentStatus.NotStarted)
            {
                userAssessment.Status = UserAssessmentStatus.InProgress;
                _userAssessmentRepository.Update(userAssessment);
            }
            else if (userAssessment.Status == UserAssessmentStatus.Submitted)
            {
                throw new InvalidOperationException("Assessment has already been submitted.");
            }
            // If InProgress, allow continuation
        }

        public async Task SubmitAssessmentAsync(int userAssessmentId, int userId)
        {
            var userAssessment = await _userAssessmentRepository.GetByIdAsync(userAssessmentId);
            if (userAssessment == null || userAssessment.UserId != userId)
            {
                throw new UnauthorizedAccessException("Assessment not found or not assigned to this user.");
            }
            if (userAssessment.Status == UserAssessmentStatus.Submitted)
            {
                throw new InvalidOperationException("Assessment has already been submitted.");
            }

            userAssessment.Status = UserAssessmentStatus.Submitted;
            _userAssessmentRepository.Update(userAssessment);

            // Trigger result generation
            await _resultService.GenerateResultAsync(userAssessmentId);
        }

        public async Task UnassignAssessmentAsync(int userAssessmentId)
        {
            var userAssessmentToDelete = await _userAssessmentRepository.GetByIdAsync(userAssessmentId);

            if (userAssessmentToDelete == null)
            {
                throw new KeyNotFoundException($"User Assessment with ID {userAssessmentId} not found.");
            }

            // BUSINESS LOGIC: Only allow unassignment if the test has NOT been started.
            // If Status > NotStarted (0), deletion should be blocked for data integrity.
            if (userAssessmentToDelete.Status != Enums.UserAssessmentStatus.NotStarted)
            {
                throw new InvalidOperationException("Cannot unassign an assessment that has already been started or submitted.");
            }

            _userAssessmentRepository.Delete(userAssessmentToDelete);
        }
    }

}
