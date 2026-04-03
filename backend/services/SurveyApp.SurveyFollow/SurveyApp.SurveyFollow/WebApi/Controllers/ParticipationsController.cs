using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.SurveyFollow.Application.Dto_s;
using SurveyApp.SurveyFollow.Application.Services;
using System.Security.Claims;
using SurveyApp.Shared.Helpers;

namespace SurveyApp.SurveyFollow.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipationsController : ControllerBase
    {
        private readonly IParticipationService _participationService;

        public ParticipationsController(IParticipationService participationService)
        {
            _participationService = participationService;
        }

        [HttpPost("submit")]
        [Authorize]
        public async Task<IActionResult> Submit(ParticipationForCreateDto dto)
        {
            var userId = HttpContext.GetUserId();
            var result = await _participationService.SubmitParticipation(dto, userId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }
    }
}
