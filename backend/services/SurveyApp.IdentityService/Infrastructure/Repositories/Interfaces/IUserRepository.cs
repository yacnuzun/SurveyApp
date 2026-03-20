using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.Shared.Persistance.Interfaces;

namespace SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<List<OperationClaim>> GetClaims(User user);
    }
}
