using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Application.Services.Interfaces;

namespace SurveyApp.IdentityService.WebApi.Controllers
{
    public class ClaimController : ControllerBase
    {
        private readonly IOperationClaimService _operationClaimService;
        private readonly IValidator<ClaimDto> _validator;

        public ClaimController(IOperationClaimService operationClaimService, IValidator<ClaimDto> validator)
        {
            _operationClaimService = operationClaimService;
            _validator = validator;
        }

        [HttpPost("add-operation-claim")]
        // [Authorize(Roles = "Admin")] // Canlýda mutlaka açýlmalý
        public async Task<IActionResult> AddOperationClaim(ClaimDto operationClaim)
        {
            var validationResult = await _validator.ValidateAsync(operationClaim);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors);

            var result = await _operationClaimService.Add(operationClaim);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}
