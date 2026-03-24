using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.SurveyManagement.Domain.Entities
{
    public class Survey : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedByAdminId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Many-to-many: Survey ↔ Question
        public ICollection<SurveyQuestion> SurveyQuestions { get; set; } = new List<SurveyQuestion>();

        // Many-to-many: Survey ↔ User (atanan kullanıcılar)
        public ICollection<SurveyUser> AssignedUsers { get; set; } = new List<SurveyUser>();
    }
    public class SurveyQuestion : IEntity
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public int Order { get; set; }

        public Survey Survey { get; set; }
        public Question Question { get; set; }
    }
    public class SurveyUser : IEntity
    {
        public int Id { get; set; }
        public int SurveyId { get; set; }
        public int UserId { get; set; }

        public Survey Survey { get; set; }
    }
}
