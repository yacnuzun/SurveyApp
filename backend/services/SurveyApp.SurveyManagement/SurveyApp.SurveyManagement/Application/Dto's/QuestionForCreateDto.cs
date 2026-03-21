using SurveyApp.Shared.Abstract;
using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class QuestionForCreateDto : IDto
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; } // Enum (Single, Multi, Text)
        public int Order { get; set; }
        public List<OptionDto> Options { get; set; } // Eğer Text ise boş gelir
    }
}
