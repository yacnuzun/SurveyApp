using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.SurveyManagement.Domain.Entities;
using SurveyApp.SurveyManagement.Infrastructure.Data;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces;

namespace SurveyApp.SurveyManagement.Infrastructure.Repositories.Implemantations
{
    public class QuestionRepository : EfRepository<Question, SurveyManagementDbContext>, IQuestionRepository
    {
        public QuestionRepository(SurveyManagementDbContext context) : base(context)
        {
        }
        public async Task<List<Question>> GetAllWithTemplatesAsync()
        {
            return await Context.Questions
                .Include(q => q.AnswerTemplate)
                    .ThenInclude(t => t.Options.OrderBy(o => o.Order))
                .ToListAsync();
        }

        public async Task<Question?> GetWithTemplateAsync(int id)
        {
            return await Context.Questions
                .Include(q => q.AnswerTemplate)
                    .ThenInclude(t => t.Options.OrderBy(o => o.Order))
                .FirstOrDefaultAsync(q => q.Id == id);
        }

    }
}
