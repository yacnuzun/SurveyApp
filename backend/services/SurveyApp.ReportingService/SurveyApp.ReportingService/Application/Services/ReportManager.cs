using SurveyApp.ReportingService.Application.Dto_s;
using SurveyApp.ReportingService.Infrastructure.Repositories;

namespace SurveyApp.ReportingService.Application.Services
{
    public class ReportManager : IReportService
    {
        private readonly IReportRepository _reportRepository;

        public ReportManager(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<SurveyReportDto> GetSurveyReportAsync(int surveyId)
        {
            var stats = await _reportRepository.GetStatsBySurveyId(surveyId);

            if (stats == null || !stats.Any()) return null;

            var report = new SurveyReportDto
            {
                SurveyId = surveyId,
                TotalParticipants = stats.GroupBy(x => x.QuestionText).FirstOrDefault()?.Sum(s => s.Count) ?? 0,
                QuestionResults = stats.GroupBy(s => s.QuestionText)
                    .Select(g => new QuestionReportDto
                    {
                        QuestionText = g.Key,
                        Labels = g.Select(x => x.OptionText).ToList(),
                        Data = g.Select(x => x.Count).ToList()
                    }).ToList()
            };

            return report;
        }

        public async Task ProcessSurveyAnswerAsync(int surveyId, string question, string option)
        {
            await _reportRepository.UpdateStats(surveyId, question, option);
        }
    }
}
