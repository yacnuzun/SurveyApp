using SurveyApp.ReportingService.Domain.Entities;

namespace SurveyApp.ReportingService.Infrastructure.Repositories
{
    public interface IReportRepository
    {
        Task<List<SurveyStatistic>> GetStatsBySurveyId(int surveyId);
        Task UpdateStats(int surveyId, string question, string option); 
    }
}
