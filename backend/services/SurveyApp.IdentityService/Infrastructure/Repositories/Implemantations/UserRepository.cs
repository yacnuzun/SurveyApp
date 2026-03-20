using Microsoft.EntityFrameworkCore;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Infrastructure.Data;
using SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces;
using SurveyApp.Shared.Persistance.Implamantations;

namespace SurveyApp.IdentityService.Infrastructure.Repositories.Implemantations
{
    public class UserRepository : EfRepository<User, IdentityDbContext>, IUserRepository
    {
        public UserRepository(IdentityDbContext context) : base(context) { }

        public async Task<List<OperationClaim>> GetClaims(User user)
        {
            var result = from operationClaim in Context.OperationClaims
                         join userOperationClaim in Context.UserOperationClaims
                             on operationClaim.Id equals userOperationClaim.OperationClaimId
                         where userOperationClaim.UserId == user.Id
                         select new OperationClaim { Id = operationClaim.Id, Name = operationClaim.Name };

            return await result.ToListAsync();
        }
    }
}
