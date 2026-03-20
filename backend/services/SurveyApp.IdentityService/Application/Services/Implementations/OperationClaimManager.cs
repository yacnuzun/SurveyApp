using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Application.Services.Interfaces;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.Shared.Persistance.Interfaces;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Implementations
{

    public class OperationClaimManager : IOperationClaimService
    {
        private readonly IOperationClaimRepository _operationClaimRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<OperationClaim> _logger;

        public OperationClaimManager(IOperationClaimRepository operationClaimRepository, IUnitOfWork unitOfWork, ILogger<OperationClaim> logger)
        {
            _operationClaimRepository = operationClaimRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IResult> Add(ClaimDto operationClaim)
        {
            try
            {
                var isController = await GetOperation(operationClaim.Role.ToString());
                if (isController.Success)
                {
                    return new ErrorResult();
                }
                var entityresult = new OperationClaim { Name = operationClaim.Role.ToString() };
                await _operationClaimRepository.AddAsync(entityresult);
                var result = await _unitOfWork.CommitAsync();
                if (result < 0)
                {
                    return new ErrorResult();
                }
                return new SuccesResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                throw;
            }

        }

        public async Task<IDataResult<OperationClaim>> GetOperation(string operation)
        {
            var result = await _operationClaimRepository.GetAsync(o => o.Name == operation);
            if (result == null)
            {
                return new ErrorDataResult<OperationClaim>();
            }
            return new SuccessDataResult<OperationClaim>(result);
        }
    }
}
