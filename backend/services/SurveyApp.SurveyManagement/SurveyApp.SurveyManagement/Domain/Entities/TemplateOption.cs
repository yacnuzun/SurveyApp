using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.SurveyManagement.Domain.Entities
{
    public class TemplateOption : IEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }            
        public int Order { get; set; }
        public int AnswerTemplateId { get; set; }

        public AnswerTemplate AnswerTemplate { get; set; }
    }

}
