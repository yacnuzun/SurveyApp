using SurveyApp.Shared.Abstract;
using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Application.Dto_s
{
    public class SurveyForCreateDto : IDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<int> QuestionIds { get; set; } = new();
        public List<int> AssignedUserIds { get; set; } = new();

    }

    public class AnswerTemplateForCreateDto : IDto
    {
        public string Name { get; set; }
        public List<TemplateOptionDto> Options { get; set; } = new();
    }
    public class AnswerTemplateForUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TemplateOptionDto> Options { get; set; } = new();
    }
    public class TemplateOptionDto : IDto
    {
        public string Text { get; set; }
        public int Order { get; set; }
    }
    public class AnswerTemplateDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int OptionCount { get; set; }
        public List<TemplateOptionDetailDto> Options { get; set; } = new();
    }
    public class TemplateOptionDetailDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int Order { get; set; }
    }
    public class QuestionForUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public int? AnswerTemplateId { get; set; }
    }
    public class SurveyForUpdateDto : IDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public List<int> QuestionIds { get; set; } = new();
        public List<int> AssignedUserIds { get; set; } = new();
    }

}
