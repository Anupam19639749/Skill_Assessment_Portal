using AutoMapper;
using System.Text.Json;
using Skill_Assessment_Portal_Backend.DTOs;
using Skill_Assessment_Portal_Backend.Models;
namespace Skill_Assessment_Portal_Backend.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User Mappings
            CreateMap<UserRegisterDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore()) // Password will be hashed by service, not directly mapped
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Set by model/DB, not DTO
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Set by model/DB, not DTO

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.RoleName)); // Map RoleName from nested Role object

            CreateMap<UserProfileUpdateDto, User>()
                .ForMember(dest => dest.FullName, opt => opt.Condition(src => src.FullName != null))
                .ForMember(dest => dest.Email, opt => opt.Condition(src => src.Email != null))
                .ForMember(dest => dest.ProfilePicturePath, opt => opt.Condition(src => src.ProfilePicturePath != null))
                .ForMember(dest => dest.Gender, opt => opt.Condition(src => src.Gender != null))
                .ForMember(dest => dest.HighestQualification, opt => opt.Condition(src => src.HighestQualification != null))
                .ForMember(dest => dest.IsEmployed, opt => opt.Condition(src => src.IsEmployed.HasValue))
                .ForMember(dest => dest.CurrentRole, opt => opt.Condition(src => src.CurrentRole != null))
                // Ignore properties that should not be updated via profile DTO
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            // Assessment Mappings
            CreateMap<AssessmentCreateDto, Assessment>()
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Will be set by service from JWT
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Set by model/DB, not DTO

            CreateMap<AssessmentUpdateDto, Assessment>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Ensure CreatedAt is not updated from DTO
                .ForMember(dest => dest.Creator, opt => opt.Ignore()) // Ensure Creator is not updated from DTO
                .ForMember(dest => dest.Questions, opt => opt.Ignore()) // Questions collection not updated via this DTO
                .ForMember(dest => dest.UserAssessments, opt => opt.Ignore()); // UserAssessments collection not updated via this DTO

            CreateMap<Assessment, AssessmentDto>()
                .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.FullName))
                .ForMember(dest => dest.QuestionCount, opt => opt.MapFrom(src => src.Questions.Count()));

            // Question Mappings
            CreateMap<QuestionCreateDto, Question>()
                 .ForMember(dest => dest.AssessmentId, opt => opt.Ignore());

            // Update the Question to QuestionDto mapping
            CreateMap<Question, QuestionDto>();


            // UserAssessment Mappings
            CreateMap<UserAssessmentAssignDto, UserAssessment>(); // Note: This DTO assigns, actual UserAssessment model fields like UserId, AssessmentId, Status will be set by service logic

            CreateMap<UserAssessment, UserAssessmentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.AssessmentTitle, opt => opt.MapFrom(src => src.Assessment.Title))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.Assessment.DurationMinutes))
                .ForMember(dest => dest.TotalMarksObtained, opt => opt.MapFrom(src => src.Result != null ? src.Result.TotalMarksObtained : (int?)null))
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Result != null ? src.Result.Percentage : (float?)null))
                .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Result != null ? src.Result.Passed : (bool?)null))
                .ForMember(dest => dest.ScheduledAt, opt => opt.MapFrom(src => src.ScheduledAt));
                

            // Submission Mappings
            CreateMap<SubmissionCreateDto, Submission>()
                .ForMember(dest => dest.SubmittedAt, opt => opt.Ignore()); // Set by model/DB, not DTO

            CreateMap<Submission, SubmissionDto>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.QuestionText))
                .ForMember(dest => dest.MaxMarks, opt => opt.MapFrom(src => src.Question.MaxMarks))
                .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.Question.CorrectAnswer));

            // Result Mappings
            CreateMap<Result, ResultDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserAssessment.UserId))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserAssessment.User.FullName))
                .ForMember(dest => dest.AssessmentId, opt => opt.MapFrom(src => src.UserAssessment.AssessmentId))
                .ForMember(dest => dest.AssessmentTitle, opt => opt.MapFrom(src => src.UserAssessment.Assessment.Title));
        }
    }
}
