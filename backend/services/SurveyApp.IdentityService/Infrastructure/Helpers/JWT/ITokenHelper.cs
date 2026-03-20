using SurveyApp.IdentityService.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;

namespace SurveyApp.IdentityService.Infrastructure.Helpers.JWT
{
    public interface ITokenHelper
    {
        AccessToken CreateToken(User user, List<OperationClaim> operationClaims);
        JwtSecurityToken ValidateTokenGetClaims(string jwtToken);

    }
}
