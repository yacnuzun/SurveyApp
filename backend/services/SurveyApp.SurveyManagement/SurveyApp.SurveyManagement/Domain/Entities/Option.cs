using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.SurveyManagement.Domain.Entities
{
    public class Option : BaseEntity
    {
        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }

        public Question Question { get; set; }
    }
}
