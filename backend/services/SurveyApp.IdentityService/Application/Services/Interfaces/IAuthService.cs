using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Infrastructure.Helpers.JWT;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto);
        Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto);
        Task<IDataResult<User>> CheckUserLogin(string mail, string role);
        Task<IResult> UserExists(string userTaxId);
        Task<IDataResult<AuthResponse>> CreateAccessToken(User user);
    }
}
