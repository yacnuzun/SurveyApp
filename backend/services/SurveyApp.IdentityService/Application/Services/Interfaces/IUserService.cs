using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Domain.Enums;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IResult> Add(User user, UserRoles role);
        Task<IDataResult<List<User>>> GetAll();
        Task<IDataResult<User>> GetById(int id);
        Task<IDataResult<List<OperationClaim>>> GetClaims(User user);
        Task<IDataResult<User>> GetByUserMail(string mail);
        Task<IDataResult<User>> GetExistUser(string email, string userName);
        Task<IDataResult<List<UserDto>>> GetAll(UserFilterDto? filter = null);
    }
}
