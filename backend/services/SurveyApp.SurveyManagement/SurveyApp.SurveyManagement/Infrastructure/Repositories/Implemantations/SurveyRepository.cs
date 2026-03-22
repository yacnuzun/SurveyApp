using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.SurveyManagement.Domain.Entities;
using SurveyApp.SurveyManagement.Infrastructure.Data;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces;

namespace SurveyApp.SurveyManagement.Infrastructure.Repositories.Implemantations
{
    public class SurveyRepository : EfRepository<Survey, SurveyManagementDbContext>, ISurveyRepository
    {
        public SurveyRepository(SurveyManagementDbContext context) : base(context)
        {
        }
        public async Task<Survey?> GetWithQuestionsAndOptionsAsync(int id)
        {
            return await Context.Surveys
                .Include(s => s.Questions)            
                    .ThenInclude(q => q.Options)      
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);
        }
    }
}
