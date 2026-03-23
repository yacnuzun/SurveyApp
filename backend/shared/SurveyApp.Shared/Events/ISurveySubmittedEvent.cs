using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SurveyApp.Shared.Events
{
    public interface ISurveySubmittedEvent
    {
        int SurveyId { get; }
        DateTime SubmittedAt { get; }
        List<AnswerMessageDto> Answers { get; }
    }
    public class AnswerMessageDto
    {
        public int QuestionId { get; set; }
        public int? OptionId { get; set; } 
        public string? TextAnswer { get; set; } 
    }
}
