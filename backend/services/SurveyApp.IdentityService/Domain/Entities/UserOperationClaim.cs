using SurveyApp.Shared.Persistance.Entities;
using System.Security.Principal;

namespace SurveyApp.IdentityService.Domain.Entities
{

    public class UserOperationClaim : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OperationClaimId { get; set; }

        // EF Core için ilişkileri bağlamak istersen:
        // public virtual User User { get; set; }
        // public virtual OperationClaim OperationClaim { get; set; }
    }
}
