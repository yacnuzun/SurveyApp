using SurveyApp.Shared.Abstract;

namespace SurveyApp.SurveyFollow.Application.Dto_s
{
    public class AnswerForCreateDto : IDto
    {
        public int QuestionId { get; set; }
        public int? OptionId { get; set; } 
        public string? TextAnswer { get; set; } 
    }
}
