using SurveyApp.IdentityService.Domain.Entities;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Interfaces
{
    public interface IUserOperationClaimService
    {
        Task<IResult> AddAsync(UserOperationClaim userOperationClaim);
        Task<IResult> AddWithoutCommitAsync(UserOperationClaim userOperationClaim);
    }
}
