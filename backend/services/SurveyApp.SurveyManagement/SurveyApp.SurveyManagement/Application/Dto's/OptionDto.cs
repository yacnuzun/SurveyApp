using SurveyApp.Shared.Abstract;

namespace SurveyApp.SurveyManagement.Application.Dto_s
{

    public class OptionDto : IDto
    {
        public string Text { get; set; }
        public int Order { get; set; }
    }
}
