using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyManagement.Domain.Entities;

namespace SurveyApp.SurveyManagement.Infrastructure.Repositories.Interfaces
{
    public interface IQuestionRepository : IRepository<Question> 
    {
        Task<List<Question>> GetAllWithTemplatesAsync();
        Task<Question?> GetWithTemplateAsync(int id);

    }
}
