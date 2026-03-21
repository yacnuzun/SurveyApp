using SurveyApp.Shared.Abstract;

namespace SurveyApp.SurveyFollow.Application.Dto_s
{
    public class ParticipationForCreateDto : IDto
    {
        public int SurveyId { get; set; }
        public List<AnswerForCreateDto> Answers { get; set; }
    }
}
