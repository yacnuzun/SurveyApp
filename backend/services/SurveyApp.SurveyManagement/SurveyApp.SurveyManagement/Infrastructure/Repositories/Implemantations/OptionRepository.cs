using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.SurveyManagement.Domain.Entities;
using SurveyApp.SurveyManagement.Infrastructure.Data;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces;

namespace SurveyApp.SurveyManagement.Infrastructure.Repositories.Implemantations
{
    public class OptionRepository : EfRepository<Option, SurveyManagementDbContext>, IOptionRepository
    {
        public OptionRepository(SurveyManagementDbContext context) : base(context)
        {
        }
    }
}
