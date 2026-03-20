using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Interfaces
{
    public interface IOperationClaimService
    {
        Task<IResult> Add(ClaimDto operationClaim);
        Task<IDataResult<OperationClaim>> GetOperation(string operation);
    }
}
