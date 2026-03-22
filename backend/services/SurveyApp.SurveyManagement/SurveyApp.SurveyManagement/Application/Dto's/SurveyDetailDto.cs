namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class SurveyDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<QuestionDetailDto> Questions { get; set; }
    }
}
