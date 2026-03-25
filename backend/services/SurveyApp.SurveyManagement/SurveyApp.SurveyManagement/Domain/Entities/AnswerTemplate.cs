using SurveyApp.Shared.Persistance.Entities;
using SurveyApp.SurveyManagement.Application.Dto_s;

namespace SurveyApp.SurveyManagement.Domain.Entities
{
    public class AnswerTemplate : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }           
        public int OptionCount { get; set; }        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<TemplateOption> Options { get; set; } = new List<TemplateOption>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
    }

}
