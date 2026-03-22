using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Application.Services;
using System.Security.Claims;

namespace SurveyApp.SurveyManagement.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SurveysController : ControllerBase
    {
        private readonly ISurveyService _surveyService;

        public SurveysController(ISurveyService surveyService)
        {
            _surveyService = surveyService;
        }

        [HttpPost("create-complex")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Create(SurveyForCreateDto surveyForCreateDto)
        {
            var adminId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var result = await _surveyService.CreateComplexSurvey(surveyForCreateDto, adminId);
            return result.Success ? Ok(result) : BadRequest(result.Message);
        }

        [HttpGet("get-all-active")]
        [Authorize] 
        public async Task<IActionResult> GetAllActive()
        {
            var result = await _surveyService.GetAllActiveSurveys();
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _surveyService.GetSurveywithId(id);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}
