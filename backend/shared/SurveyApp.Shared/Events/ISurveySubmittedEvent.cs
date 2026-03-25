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
        List<IAnswerMessage> Answers { get; }
    }
    public interface IAnswerMessage
    {
        int QuestionId { get; }
        string QuestionText { get; }
        int? OptionId { get; }
        string? OptionText { get; }
        string? TextAnswer { get; }
    }
}
