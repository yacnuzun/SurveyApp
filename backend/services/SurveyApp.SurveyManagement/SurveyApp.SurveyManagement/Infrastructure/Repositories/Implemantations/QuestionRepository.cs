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
    }
}
