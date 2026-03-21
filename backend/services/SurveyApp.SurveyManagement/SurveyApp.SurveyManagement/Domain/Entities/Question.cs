using SurveyApp.Shared.Persistance.Entities;
using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Domain.Entities
{
    public class Question : BaseEntity
    {
        public int SurveyId { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; } // Enum: Single, Multi, Text
        public int Order { get; set; }

        public Survey Survey { get; set; }
        public ICollection<Option> Options { get; set; }
    }
}
