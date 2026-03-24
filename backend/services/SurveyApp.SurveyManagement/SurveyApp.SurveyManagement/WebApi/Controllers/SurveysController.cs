using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.Shared.Abstract;
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

        // Mevcut — eski akýþ korunuyor
        [HttpPost("create-complex")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateComplex(SurveyForCreateDto surveyForCreateDto)
        {
            var adminId = Convert.ToInt32(User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var result = await _surveyService.CreateAsync(surveyForCreateDto, adminId);
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, SurveyForUpdateDto dto)
        {
            dto.Id = id;
            var result = await _surveyService.UpdateAsync(dto);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _surveyService.DeleteAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPatch("{id}/toggle-active")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var result = await _surveyService.ToggleActiveAsync(id);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpGet("my-surveys")]
        [Authorize]
        public async Task<IActionResult> GetMySurveys()
        {
            var userId = Convert.ToInt32(User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)!.Value);
            var result = await _surveyService.GetAssignedSurveysForUser(userId);
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }
    }
}
