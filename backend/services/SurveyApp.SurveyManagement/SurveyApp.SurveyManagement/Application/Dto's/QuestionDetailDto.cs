using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class QuestionDetailDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public int? AnswerTemplateId { get; set; }
        public string? AnswerTemplateName { get; set; }
        public List<TemplateOptionDetailDto> Options { get; set; } = new();
    }
}
