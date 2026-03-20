using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.IdentityService.Domain.Entities
{
    public class OperationClaim : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
