using AutoMapper;
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
                .ForMember(dest => dest.AssessmentId, opt => opt.Ignore()); // AssessmentId from route/context
            CreateMap<QuestionUpdateDto, Question>();
            CreateMap<Question, QuestionDto>();

            // UserAssessment Mappings
            CreateMap<UserAssessmentAssignDto, UserAssessment>(); // Note: This DTO assigns, actual UserAssessment model fields like UserId, AssessmentId, Status will be set by service logic

            CreateMap<UserAssessment, UserAssessmentDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.AssessmentTitle, opt => opt.MapFrom(src => src.Assessment.Title))
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.Assessment.DurationMinutes))
                .ForMember(dest => dest.TotalMarksObtained, opt => opt.MapFrom(src => src.Result != null ? src.Result.TotalMarksObtained : (int?)null))
                .ForMember(dest => dest.Percentage, opt => opt.MapFrom(src => src.Result != null ? src.Result.Percentage : (float?)null))
                .ForMember(dest => dest.Passed, opt => opt.MapFrom(src => src.Result != null ? src.Result.Passed : (bool?)null));

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
