using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Application.Services.Interfaces;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Domain.Enums;
using SurveyApp.IdentityService.Infrastructure.Repositories.Interfaces;
using SurveyApp.Shared.Constant;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.Shared.Persistance.Interfaces;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Implementations
{
    public class UserManager : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserOperationClaimService _userOperationClaimService;
        private readonly IOperationClaimService _operationClaim;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UserManager> _logger;

        public UserManager(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<UserManager> logger,
            IUserOperationClaimService userOperationClaimService,
            IOperationClaimService operationClaim)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _userOperationClaimService = userOperationClaimService;
            _operationClaim = operationClaim;
        }

        public async Task<IDataResult<List<OperationClaim>>> GetClaims(User user)
        {
            try
            {
                var result = await _userRepository.GetClaims(user);
                if (result == null)
                {
                    return new ErrorDataResult<List<OperationClaim>>();
                }
                return new SuccessDataResult<List<OperationClaim>>(result);
            }
            catch (Exception ex)
            {

                _logger.LogError($"{ex.InnerException}/{ex.Message}/{ex.Source}");
                throw;
            }
        }
        public async Task<IResult> Add(User user, UserRoles role) 
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _userRepository.AddAsync(user);
                await _unitOfWork.CommitAsync(); 

                var resultOperation = await _operationClaim.GetOperation(role.ToString());

                if (!resultOperation.Success || resultOperation.Data == null)
                {
                    
                    throw new Exception(Messages.RoleNotFound);
                }

                var userOperationClaim = new UserOperationClaim
                {
                    OperationClaimId = resultOperation.Data.Id,
                    UserId = user.Id
                };

                await _userOperationClaimService.AddWithoutCommitAsync(userOperationClaim);

                await _unitOfWork.CommitAsync();
                await _unitOfWork.CommitTransactionAsync();

                return new SuccesResult(Messages.UserRegistered);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                _logger.LogError($"{ex.Message}");
                throw;
            }
        }
        public async Task<IDataResult<List<User>>> GetAll()
        {
            try
            {
                var result = await _userRepository.ListAsync();
                if (result == null)
                {
                    return new ErrorDataResult<List<User>>();
                }
                return new SuccessDataResult<List<User>>(result.ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.InnerException}/{ex.Message}/{ex.Source}");
                throw;
            }

        }

        public async Task<IDataResult<List<UserDto>>> GetAll(UserFilterDto? filter = null)
        {
            try
            {
                var result = await _userRepository.ListAsync();
                if (result == null)
                    return new ErrorDataResult<List<UserDto>>();

                var users = result.ToList();

                if (!string.IsNullOrWhiteSpace(filter?.Search))
                {
                    var search = filter.Search.ToLower();
                    users = users.Where(u =>
                        u.UserName.ToLower().Contains(search) ||
                        u.Email.ToLower().Contains(search)
                    ).ToList();
                }

                return new SuccessDataResult<List<UserDto>>(
                    users.Select( s => {
                        return new UserDto
                        {
                            UserName = s.UserName,
                            Email = s.Email,
                            UserId = s.Id,
                            Status = s.Status
                        };
                }).ToList());
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.InnerException}/{ex.Message}/{ex.Source}");
                throw;
            }
        }


        public async Task<IDataResult<User>> GetById(int id)
        {
            try
            {
                var result = await _userRepository.GetAsync(u => u.Id == id && u.Status == true);
                if (result == null)
                {
                    return new ErrorDataResult<User>();
                }
                return new SuccessDataResult<User>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.InnerException}/{ex.Message}/{ex.Source}");
                throw;
            }
        }

        public async Task<IDataResult<User>> GetByUserMail(string mail)
        {
            try
            {
                var result = await _userRepository.GetAsync(u => u.Email == mail && u.Status == true);
                if (result == null)
                {
                    return new ErrorDataResult<User>();
                }
                return new SuccessDataResult<User>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.InnerException}/{ex.Message}/{ex.Source}");
                throw;
            }


        }

        public async Task<IDataResult<User>> GetExistUser(string email, string userName)
        {
            try
            {
                var entity = await _userRepository.GetAsync(u => u.Email == email || u.UserName == userName);
                if (entity == null)
                {
                    return new ErrorDataResult<User>();
                }
                return new SuccessDataResult<User>(entity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }

        }
    }
}
