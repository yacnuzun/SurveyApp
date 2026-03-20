using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Infrastructure.Data;
using SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces;
using SurveyApp.Shared.Persistance.Implamantations;

namespace SurveyApp.IdentityService.Infrastructure.Repositories.Implemantations
{
    public class UserOperationClaimRepository : EfRepository<UserOperationClaim, IdentityDbContext>, IUserOperationClaimRepository
    {
        public UserOperationClaimRepository(IdentityDbContext context) : base(context)
        {
        }
    }
}
