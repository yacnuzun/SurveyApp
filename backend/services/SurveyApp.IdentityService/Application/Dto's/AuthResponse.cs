using SurveyApp.IdentityService.Infrastructure.Helpers.JWT;

namespace SurveyApp.IdentityService.Application.Dto_s
{
    public class AuthResponse
    {
        public AccessToken Token { get; set; }
        public string Email { get; set; }
        public List<string> Role { get; set; } 
        public int UserId { get; set; } 
        public string UserName { get; set; }
    }
}
