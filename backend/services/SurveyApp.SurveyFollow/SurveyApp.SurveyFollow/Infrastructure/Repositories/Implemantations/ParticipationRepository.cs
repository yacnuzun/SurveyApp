using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.SurveyFollow.Domain.Entities;
using SurveyApp.SurveyFollow.Infrastructure.Data;
using SurveyApp.SurveyFollow.Infrastructure.Repositories.Interfaces;

namespace SurveyApp.SurveyFollow.Infrastructure.Repositories.Implemantations
{
    public class ParticipationRepository : EfRepository<Participation, ParticipationDbContext>, IParticipationRepository
    {
        public ParticipationRepository(ParticipationDbContext context) : base(context) { }

        public async Task<bool> HasUserParticipatedAsync(int surveyId, int userId)
        {
            var result = await GetAsync(p => p.SurveyId == surveyId && p.UserId == userId);
            return result != null;
        }
    }

}
