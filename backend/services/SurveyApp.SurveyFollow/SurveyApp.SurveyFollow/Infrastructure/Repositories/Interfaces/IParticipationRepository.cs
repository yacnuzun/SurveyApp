using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.Shared.Persistance.Interfaces;
using SurveyApp.SurveyFollow.Domain.Entities;
using SurveyApp.SurveyFollow.Infrastructure.Data;
using SurveyApp.SurveyFollow.Infrastructure.Repositories.Implemantations;

namespace SurveyApp.SurveyFollow.Infrastructure.Repositories.Interfaces
{
    public interface IParticipationRepository : IRepository<Participation>
    {
        Task<bool> HasUserParticipatedAsync(int surveyId, int userId);
    }
}
