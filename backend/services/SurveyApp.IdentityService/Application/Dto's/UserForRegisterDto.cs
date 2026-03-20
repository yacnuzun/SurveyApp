using SurveyApp.IdentityService.Domain.Enums;
using SurveyApp.Shared.Abstract;

namespace SurveyApp.IdentityService.Application.Dto_s
{
    public class UserForRegisterDto : IDto
    {
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public UserRoles Role { get; set; }
    }
}
