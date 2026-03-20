using SurveyApp.IdentityService.Domain.Enums;
using SurveyApp.Shared.Abstract;

namespace SurveyApp.IdentityService.Application.Dto_s
{
    public class ClaimDto : IDto
    {
        public UserRoles Role { get; set; }
    }
}
