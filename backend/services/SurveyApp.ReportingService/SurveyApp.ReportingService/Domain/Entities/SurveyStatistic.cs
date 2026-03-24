using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.ReportingService.Domain.Entities
{
    public class SurveyStatistic:IEntity
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public string QuestionText { get; set; }
        public string OptionText { get; set; }
        public int Count { get; set; } 
    }
}
