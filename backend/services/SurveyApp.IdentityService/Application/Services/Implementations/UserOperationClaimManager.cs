using SurveyApp.IdentityService.Application.Services.Interfaces;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.Shared.Persistance.Interfaces;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Implementations
{
    public class UserOperationClaimManager : IUserOperationClaimService
    {
        private readonly IUserOperationClaimRepository _userClaimRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserOperationClaim> _logger;

        public UserOperationClaimManager(IUserOperationClaimRepository userClaimRepository, IUnitOfWork unitOfWork, ILogger<UserOperationClaim> logger)
        {
            _userClaimRepository = userClaimRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IResult> AddAsync(UserOperationClaim userOperationClaim)
        {
            try
            {
                await _userClaimRepository.AddAsync(userOperationClaim);
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

        public async Task<IResult> AddWithoutCommitAsync(UserOperationClaim userOperationClaim)
        {
            try
            {
                await _userClaimRepository.AddAsync(userOperationClaim);

                return new SuccesResult();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}", ex);
                throw;
            }

        }

    }
}
