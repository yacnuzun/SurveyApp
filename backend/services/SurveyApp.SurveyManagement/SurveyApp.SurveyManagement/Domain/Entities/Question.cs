using SurveyApp.Shared.Persistance.Entities;
using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Domain.Entities
{
    public class Question : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }      
        public int Order { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Şablon ilişkisi (Text tipi sorular için null olabilir)
        public int? AnswerTemplateId { get; set; }
        public AnswerTemplate? AnswerTemplate { get; set; }

        // Anket ilişkisi (many-to-many)
        public ICollection<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();
    }

}
