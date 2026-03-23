using Microsoft.EntityFrameworkCore;
using SurveyApp.ReportingService.Domain.Entities;
using SurveyApp.ReportingService.Infrastructure.Data;

namespace SurveyApp.ReportingService.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ReportingDbContext _context;

        public ReportRepository(ReportingDbContext context)
        {
            _context = context;
        }

        // Admin paneli için istatistikleri getirir
        public async Task<List<SurveyStatistic>> GetStatsBySurveyId(int surveyId)
        {
            return await _context.Statistics
                .Where(s => s.SurveyId == surveyId)
                .OrderBy(s => s.QuestionText) // Sorulara göre sıralı gelsin
                .ToListAsync();
        }

        // RabbitMQ'dan mesaj geldiğinde istatistiği güncelleyen metod
        public async Task UpdateStats(int surveyId, string question, string option)
        {
            // Bu anket, bu soru ve bu şık için daha önce bir kayıt var mı?
            var stat = await _context.Statistics
                .FirstOrDefaultAsync(s => s.SurveyId == surveyId
                                     && s.QuestionText == question
                                     && s.OptionText == option);

            if (stat != null)
            {
                // Varsa sayacı 1 artır
                stat.Count++;
                _context.Statistics.Update(stat);
            }
            else
            {
                // Yoksa yeni bir satır oluştur (İlk kez bu şık işaretlendi)
                await _context.Statistics.AddAsync(new SurveyStatistic
                {
                    SurveyId = surveyId,
                    QuestionText = question,
                    OptionText = option,
                    Count = 1
                });
            }

            await _context.SaveChangesAsync();
        }
    }
    public interface IReportRepository
    {
        Task<List<SurveyStatistic>> GetStatsBySurveyId(int surveyId);
        Task UpdateStats(int surveyId, string question, string option); 
    }
}
