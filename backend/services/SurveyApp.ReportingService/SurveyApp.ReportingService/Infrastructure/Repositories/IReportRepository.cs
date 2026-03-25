using SurveyApp.ReportingService.Domain.Entities;
using SurveyApp.Shared.Persistance.Interfaces;

namespace SurveyApp.ReportingService.Infrastructure.Repositories
{
    public interface IReportRepository : IRepository<SurveyStatistic>
    {
        Task<List<SurveyStatistic>> GetBySurveyIdAsync(int surveyId);
    }

}
