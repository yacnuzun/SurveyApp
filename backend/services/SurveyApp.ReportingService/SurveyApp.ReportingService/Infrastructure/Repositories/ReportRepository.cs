using Microsoft.EntityFrameworkCore;
using SurveyApp.ReportingService.Domain.Entities;
using SurveyApp.ReportingService.Infrastructure.Data;
using SurveyApp.Shared.Persistance.Implamantations;

namespace SurveyApp.ReportingService.Infrastructure.Repositories
{
    public class ReportRepository : EfRepository<SurveyStatistic, ReportingDbContext>, IReportRepository
    {
        public ReportRepository(ReportingDbContext context) : base(context) { }

        public async Task<List<SurveyStatistic>> GetBySurveyIdAsync(int surveyId)
        {
            return (await ListAsync(s => s.SurveyId == surveyId)).ToList();
        }
    }

}
