using SurveyApp.Shared.Abstract;

namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class SurveyForCreateDto : IDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<QuestionForCreateDto> Questions { get; set; }
    }
}
