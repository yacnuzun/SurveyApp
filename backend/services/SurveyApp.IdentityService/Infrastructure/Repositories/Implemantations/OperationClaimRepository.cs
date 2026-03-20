using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Infrastructure.Data;
using SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces;
using SurveyApp.Shared.Persistance.Implamantations;

namespace SurveyApp.IdentityService.Infrastructure.Repositories.Implemantations
{
    public class OperationClaimRepository : EfRepository<OperationClaim, IdentityDbContext>, IOperationClaimRepository
    {
        public OperationClaimRepository(IdentityDbContext context) : base(context)
        {
        }
    }
}
