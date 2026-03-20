using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.IdentityService.Domain.Entities
{
    public class User : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }

        // Opsiyonel: Navigation Property
        // public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; }
    }
}
