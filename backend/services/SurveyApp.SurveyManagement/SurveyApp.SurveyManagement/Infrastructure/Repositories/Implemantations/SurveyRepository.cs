using Microsoft.EntityFrameworkCore;
using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.SurveyManagement.Domain.Entities;
using SurveyApp.SurveyManagement.Infrastructure.Data;
using SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces;

namespace SurveyApp.SurveyManagement.Infrastructure.Repositories.Implemantations
{
    public class SurveyRepository : EfRepository<Survey, SurveyManagementDbContext>, ISurveyRepository
    {
        public SurveyRepository(SurveyManagementDbContext context) : base(context)
        {
        }
        public async Task<Survey?> GetWithQuestionsAsync(int id)
        {
            return await Context.Surveys
        .Include(s => s.SurveyQuestions)
            .ThenInclude(sq => sq.Question)
                .ThenInclude(q => q.AnswerTemplate)
                    .ThenInclude(t => t.Options.OrderBy(o => o.Order))
        .Include(s => s.AssignedUsers)  
        .FirstOrDefaultAsync(s => s.Id == id);

        }

        public async Task<List<Survey>> GetAllWithDetailsAsync()
        {
            return await Context.Surveys
        .Include(s => s.SurveyQuestions)
            .ThenInclude(sq => sq.Question)
                .ThenInclude(q => q.AnswerTemplate)
                    .ThenInclude(t => t.Options.OrderBy(o => o.Order))
        .Include(s => s.AssignedUsers)
        .Where(s => s.IsActive && s.EndDate > DateTime.UtcNow)
        .ToListAsync();

        }

        public async Task<List<Survey>> GetAssignedSurveysForUserAsync(int userId)
        {
            return await Context.Surveys
        .Include(s => s.SurveyQuestions)
            .ThenInclude(sq => sq.Question)
                .ThenInclude(q => q.AnswerTemplate)
                    .ThenInclude(t => t.Options.OrderBy(o => o.Order))
        .Include(s => s.AssignedUsers)  
        .Where(s =>
            s.IsActive &&
            s.StartDate <= DateTime.UtcNow &&
            s.EndDate >= DateTime.UtcNow &&
            s.AssignedUsers.Any(u => u.UserId == userId))
        .ToListAsync();

        }

        public async Task<List<Survey>> GetAllWithDetailsForAdminAsync()
        {
            return await Context.Surveys
                .Include(s => s.SurveyQuestions)
                    .ThenInclude(sq => sq.Question)
                        .ThenInclude(q => q.AnswerTemplate)
                            .ThenInclude(t => t.Options.OrderBy(o => o.Order))
                .Include(s => s.AssignedUsers)
                .ToListAsync(); // ← filtre yok
        }
    }
    public class AnswerTemplateRepository : EfRepository<AnswerTemplate, SurveyManagementDbContext>, IAnswerTemplateRepository
    {
        public AnswerTemplateRepository(SurveyManagementDbContext context) : base(context) { }

        public async Task<AnswerTemplate?> GetWithOptionsAsync(int id)
        {
            return await Context.AnswerTemplates
                .Include(t => t.Options.OrderBy(o => o.Order))
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<AnswerTemplate>> GetAllWithOptionsAsync()
        {
            return await Context.AnswerTemplates
                .Include(t => t.Options.OrderBy(o => o.Order))
                .ToListAsync();
        }
    }
    public class SurveyUserRepository : EfRepository<SurveyUser, SurveyManagementDbContext>, ISurveyUserRepository
    {
        public SurveyUserRepository(SurveyManagementDbContext context) : base(context) { }
    }
    public class SurveyQuestionRepository : EfRepository<SurveyQuestion, SurveyManagementDbContext>, ISurveyQuestionRepository
    {
        public SurveyQuestionRepository(SurveyManagementDbContext context) : base(context) { }
    }
    public class TemplateOptionRepository : EfRepository<TemplateOption, SurveyManagementDbContext>, ITemplateOptionRepository
    {
        public TemplateOptionRepository(SurveyManagementDbContext context) : base(context) { }
    }

}
