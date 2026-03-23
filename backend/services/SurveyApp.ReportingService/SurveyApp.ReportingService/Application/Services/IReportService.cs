using SurveyApp.ReportingService.Application.Dto_s;

namespace SurveyApp.ReportingService.Application.Services
{
    public interface IReportService
    {
        Task<SurveyReportDto> GetSurveyReportAsync(int surveyId);
        Task ProcessSurveyAnswerAsync(int surveyId, string question, string option);
    }
}
