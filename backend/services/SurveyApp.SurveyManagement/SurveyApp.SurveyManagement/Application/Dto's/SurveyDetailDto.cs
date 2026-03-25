namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class SurveyDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<int> AssignedUserIds { get; set; } = new(); 
        public List<QuestionDetailDto> Questions { get; set; } = new();
    }
}
