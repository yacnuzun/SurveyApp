using MassTransit;
using SurveyApp.ReportingService.Application.Services;
using SurveyApp.Shared.Events;

namespace SurveyApp.ReportingService.Infrastructure.Helpers.Consumer
{
    public class SurveySubmittedConsumer : IConsumer<ISurveySubmittedEvent>
    {
        private readonly ILogger<SurveySubmittedConsumer> _logger;
        private readonly IReportService _reportService;

        public SurveySubmittedConsumer(ILogger<SurveySubmittedConsumer> logger, IReportService reportService)
        {
            _logger = logger;
            _reportService = reportService;
        }

        public async Task Consume(ConsumeContext<ISurveySubmittedEvent> context)
        {
            var data = context.Message;

            _logger.LogInformation("SurveySubmitted event alındı. SurveyId: {SurveyId}", data.SurveyId);

            foreach (var answer in data.Answers)
            {
                // Text tipi sorular raporda gösterilmez, sadece seçimli olanlar
                if (string.IsNullOrEmpty(answer.OptionText)) continue;

                await _reportService.ProcessSurveyAnswerAsync(
                    data.SurveyId,
                    answer.QuestionText,
                    answer.OptionText);
            }

        }
    }
}
