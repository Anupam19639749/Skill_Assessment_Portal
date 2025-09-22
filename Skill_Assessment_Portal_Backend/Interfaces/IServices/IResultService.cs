//Purpose: Business logic for generating, retrieving, and(if manual evaluation) updating assessment results.

using Skill_Assessment_Portal_Backend.DTOs;

namespace Skill_Assessment_Portal_Backend.Interfaces.IServices

{
    public interface IResultService
    {
        Task<ResultDto> GetResultByUserAssessmentIdAsync(int userAssessmentId, int userId); // userId for auth check
        Task<ResultDto> GenerateResultAsync(int userAssessmentId); // Called after submission or manual eval
        Task UpdateResultManualEvaluationAsync(int submissionId, int marksObtained); // For evaluator to mark descriptive questions
    }
}
