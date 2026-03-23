using MassTransit;
using SurveyApp.ReportingService.Application.Services;
using SurveyApp.Shared.Events;

namespace SurveyApp.ReportingService.Infrastructure.Helpers.Consumer
{
    public class SurveySubmittedConsumer : IConsumer<ISurveySubmittedEvent>
    {
        private readonly ILogger<SurveySubmittedConsumer> _logger;
        // Burada kendi DB Context'ini veya Service'ini enjekte edeceksin
        private readonly IReportService _reportService;

        public SurveySubmittedConsumer(ILogger<SurveySubmittedConsumer> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        public async Task Consume(ConsumeContext<ISurveySubmittedEvent> context)
        {
            var data = context.Message;

            var processDto = new
            {
                SurveyId = data.SurveyId,
                Answers = data.Answers.Select(a => new
                {
                    QuestionId = a.QuestionId,
                    OptionId = a.OptionId,
                    TextAnswer = a.TextAnswer
                }).ToList()
            };

            _logger.LogInformation($"Yeni anket cevabı alındı! SurveyId: {data.SurveyId}");

            // TODO: Burada veritabanı güncelleme mantığını çalıştıracağız
            foreach (var item in processDto.Answers)
            {
                await _reportService.ProcessSurveyAnswerAsync(processDto.SurveyId, item.QuestionId.ToString(),item.TextAnswer);
            }
            
        }
    }
}
