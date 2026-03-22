using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Application.Services.Interfaces;
using SurveyApp.IdentityService.Domain.Entities;
using SurveyApp.IdentityService.Infrastructure.Helpers.JWT;
using SurveyApp.Shared.Constant;
using SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels;
using SurveyApp.Shared.Helpers.Security.Hashing;
using IResult = SurveyApp.Shared.Helpers.ResponseModels.GenericResultModels.IResult;

namespace SurveyApp.IdentityService.Application.Services.Implementations
{
    public class AuthManager : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ITokenHelper _tokenHelper;
        private readonly ILogger<AuthManager> _log;

        public AuthManager(IUserService userService,
            ITokenHelper tokenHelper,
            ILogger<AuthManager> log)
        {
            _userService = userService;
            _tokenHelper = tokenHelper;
            _log = log;
        }
        public async Task<IDataResult<User>> Register(UserForRegisterDto userForRegisterDto)
        {
            try
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);
                var user = new User
                {
                    UserName = userForRegisterDto.UserName,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Email = userForRegisterDto.Email,
                    Status = true
                };
                await _userService.Add(user, userForRegisterDto.Role);

                return new SuccessDataResult<User>(user, Messages.UserRegistered);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }

        }
        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            try
            {
                var userToCheck = await _userService.GetByUserMail(userForLoginDto.Email);
                if (!userToCheck.Success)
                {
                    return new ErrorDataResult<User>(Messages.UserNotFound);
                }

                if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.PasswordHash, userToCheck.Data.PasswordSalt))
                {
                    return new ErrorDataResult<User>(Messages.PasswordError);
                }

                return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }

        }

        public async Task<IResult> UserExists(string mail)
        {
            try
            {
                var result = await _userService.GetByUserMail(mail);
                if (result.Success)
                {
                    return new ErrorResult(Messages.UserAlreadyExists);
                }
                return new SuccesResult();
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }

        }

        public async Task<IDataResult<AuthResponse>> CreateAccessToken(User user)
        {
            var claims = await _userService.GetClaims(user);
            var accessToken = _tokenHelper.CreateToken(user, claims.Data);
            return new SuccessDataResult<AuthResponse>(
                new AuthResponse { 
                    UserId = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                Role=claims.Data.Select(c=> c.Name).ToList(),
                Token= accessToken}, 
                Messages.AccessTokenCreated);
        }

        public async Task<IDataResult<User>> CheckUserLogin(string mail, string role)
        {
            try
            {
                var userToCheck = await _userService.GetByUserMail(mail);
                if (userToCheck == null)
                {
                    return new ErrorDataResult<User>(Messages.UserNotFound);
                }

                var listclaims = await _userService.GetClaims(userToCheck.Data);

                if (!listclaims.Data.Exists(l => l.Name.ToLower().Contains(role.ToLower())))
                {
                    return new ErrorDataResult<User>(Messages.AccessWarning);
                }

                return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
            }
            catch (Exception ex)
            {
                _log.LogError(ex.Message);
                throw;
            }

        }
    }
}
