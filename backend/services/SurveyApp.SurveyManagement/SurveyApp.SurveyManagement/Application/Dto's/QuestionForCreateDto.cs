using SurveyApp.Shared.Abstract;
using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class QuestionForCreateDto : IDto
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public int? AnswerTemplateId { get; set; }
    }
}
