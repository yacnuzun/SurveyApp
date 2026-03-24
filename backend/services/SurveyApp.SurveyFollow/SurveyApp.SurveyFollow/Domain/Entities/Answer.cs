using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.SurveyFollow.Domain.Entities
{
    public class Answer : IEntity
    {
        public int Id { get; set; }
        public int ParticipationId { get; set; }
        public int QuestionId { get; set; }      
        public int? OptionId { get; set; }       
        public string? TextAnswer { get; set; }
        public Participation Participation { get; set; }
    }

}
