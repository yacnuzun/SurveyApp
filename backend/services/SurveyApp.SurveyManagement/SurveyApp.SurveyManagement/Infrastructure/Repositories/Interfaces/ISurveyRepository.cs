using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyManagement.Domain.Entities;

namespace SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces
{
    public interface ISurveyRepository : IRepository<Survey> 
    {
            Task<Survey?> GetWithQuestionsAsync(int id);
            Task<List<Survey>> GetAllWithDetailsAsync();         
            Task<List<Survey>> GetAssignedSurveysForUserAsync(int userId);


    }
    public interface IAnswerTemplateRepository : IRepository<AnswerTemplate>
    {
        Task<AnswerTemplate?> GetWithOptionsAsync(int id);
        Task<List<AnswerTemplate>> GetAllWithOptionsAsync();
    }
    public interface ISurveyUserRepository : IRepository<SurveyUser> { }
    public interface ISurveyQuestionRepository : IRepository<SurveyQuestion> { }
    public interface ITemplateOptionRepository : IRepository<TemplateOption> { }
}
