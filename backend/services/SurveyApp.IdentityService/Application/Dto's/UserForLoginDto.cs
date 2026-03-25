using SurveyApp.Shared.Abstract;

namespace SurveyApp.IdentityService.Application.Dto_s
{
    public class UserForLoginDto : IDto
    {
        public string Email { get; set; } 
        public string Password { get; set; }
    }
}
