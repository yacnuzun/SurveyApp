using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.SurveyFollow.Domain.Entities
{
    public class Participation : IEntity
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int UserId { get; set; }
        public DateTime ParticipationDate { get; set; } = DateTime.UtcNow;

        public ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }

}
