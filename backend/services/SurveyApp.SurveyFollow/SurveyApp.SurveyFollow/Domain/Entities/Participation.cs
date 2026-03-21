using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.SurveyFollow.Domain.Entities
{
    public class Participation : BaseEntity
    {
        public int SurveyId { get; set; } // Management servisindeki Survey ID
        public int UserId { get; set; }   // Identity servisindeki User ID
        public DateTime ParticipationDate { get; set; } = DateTime.UtcNow;

        // Navigation Property
        public ICollection<Answer> Answers { get; set; } = new HashSet<Answer>();
    }
}
