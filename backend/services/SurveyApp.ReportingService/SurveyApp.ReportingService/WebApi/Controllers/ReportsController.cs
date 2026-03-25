using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SurveyApp.ReportingService.Application.Services;

namespace SurveyApp.ReportingService.WebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("{surveyId}")]
        public async Task<IActionResult> GetSurveyReport(int surveyId)
        {
            var result = await _reportService.GetSurveyReportAsync(surveyId);
            if (result == null) return NotFound("Bu anket için henüz veri bulunamadý.");

            return Ok(result);
        }
        [HttpPost("test-process-answer")]
        [AllowAnonymous] 
        public async Task<IActionResult> TestProcessAnswer(int surveyId, string question, string option)
        {
            try
            {
                await _reportService.ProcessSurveyAnswerAsync(surveyId, question, option);
                return Ok(new { message = "Ýþlem baþarýlý, DB kontrol edilebilir." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
