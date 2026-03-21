using SurveyApp.Shared.Persistance.Entities;

namespace SurveyApp.SurveyManagement.Domain.Entities
{
    public class Survey : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedBy { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        // Navigation Property
        public ICollection<Question> Questions { get; set; }
    }
}
