using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Application.Services.Interfaces;
using SurveyApp.IdentityService.Infrastructure.Helpers.JWT;
using SurveyApp.Shared.Constant;
using SurveyApp.Shared.Dto_s;
using System.Security.Claims;

namespace SurveyApp.IdentityService.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IValidator<UserForRegisterDto> _registerValidator;

        public AccountController(IAuthService authService,
            IValidator<UserForRegisterDto> registerValidator)
        {
            _authService = authService;
            _registerValidator = registerValidator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userToLogin = await _authService.Login(userForLoginDto);
            if (!userToLogin.Success) return BadRequest(userToLogin.Message);

            var result = await _authService.CreateAccessToken(userToLogin.Data);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var validationResult = await _registerValidator.ValidateAsync(userForRegisterDto);
            if (!validationResult.IsValid) return BadRequest(validationResult.Errors);

            var result = await _authService.Register(userForRegisterDto);
            if (!result.Success) return BadRequest(result.Message);


            return Ok(result.Data);
        }

        [HttpGet("me")] 
        [Authorize]
        public async Task<IActionResult> GetMyDetails(string targetRole)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(userEmail)) return Unauthorized();

            var result = await _authService.CheckUserLogin(userEmail, targetRole);

            if (!result.Success) return Forbid();

            return Ok(new { Email = userEmail, Status = "Verified", Role = targetRole });
        }
    }
}
