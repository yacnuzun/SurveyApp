using SurveyApp.Shared.Persistance.Implamantations;
using SurveyApp.SurveyFollow.Domain.Entities;
using SurveyApp.SurveyFollow.Infrastructure.Data;
using SurveyApp.SurveyFollow.Infrastructure.Repositories.Interfaces;

namespace SurveyApp.SurveyFollow.Infrastructure.Repositories.Implemantations
{
    public class AnswerRepository : EfRepository<Answer, ParticipationDbContext>, IAnswerRepository
    {
        public AnswerRepository(ParticipationDbContext context) : base(context)
        {
        }
    }
}
