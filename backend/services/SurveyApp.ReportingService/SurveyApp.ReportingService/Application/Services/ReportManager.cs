using SurveyApp.ReportingService.Application.Dto_s;
using SurveyApp.ReportingService.Domain.Entities;
using SurveyApp.ReportingService.Infrastructure.Repositories;
using SurveyApp.Shared.Persistance.Interfaces;

namespace SurveyApp.ReportingService.Application.Services
{
    public class ReportManager : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ReportManager> _log;

        public ReportManager(
            IReportRepository reportRepository,
            IUnitOfWork unitOfWork,
            ILogger<ReportManager> log)
        {
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _log = log;
        }

        public async Task<SurveyReportDto?> GetSurveyReportAsync(int surveyId)
        {
            var stats = await _reportRepository.GetBySurveyIdAsync(surveyId);
            if (!stats.Any()) return null;

            // QuestionText bazında grupla
            var grouped = stats
                .GroupBy(s => s.QuestionText)
                .Select(g => new QuestionReportDto
                {
                    QuestionText = g.Key,
                    Labels = g.Select(x => x.OptionText).ToList(),
                    Data = g.Select(x => x.Count).ToList()
                }).ToList();

            return new SurveyReportDto
            {
                SurveyId = surveyId,
                TotalParticipants = stats
                    .GroupBy(s => s.QuestionText)
                    .First().Sum(x => x.Count),
                QuestionResults = grouped
            };
        }

        public async Task ProcessSurveyAnswerAsync(int surveyId, string questionText, string optionText)
        {
            try
            {
                var existing = await _reportRepository.GetAsync(s =>
                    s.SurveyId == surveyId &&
                    s.QuestionText == questionText &&
                    s.OptionText == optionText);

                if (existing != null)
                {
                    existing.Count++;
                    _reportRepository.Update(existing);
                }
                else
                {
                    await _reportRepository.AddAsync(new SurveyStatistic
                    {
                        SurveyId = surveyId,
                        QuestionText = questionText,
                        OptionText = optionText,
                        Count = 1
                    });
                }

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "ProcessSurveyAnswer hata: {Message}", ex.Message);
                throw;
            }
        }
    }
}
