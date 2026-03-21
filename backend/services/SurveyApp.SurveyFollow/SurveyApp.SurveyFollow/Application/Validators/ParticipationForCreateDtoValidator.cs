using FluentValidation;
using SurveyApp.SurveyFollow.Application.Dto_s;

namespace SurveyApp.SurveyFollow.Application.Validators
{
    public class ParticipationForCreateDtoValidator : AbstractValidator<ParticipationForCreateDto>
    {
        public ParticipationForCreateDtoValidator()
        {
            RuleFor(x => x.SurveyId).GreaterThan(0);
            RuleFor(x => x.Answers).NotEmpty().WithMessage("En az bir soruya cevap verilmelidir.");
            RuleForEach(x => x.Answers).ChildRules(answer => {
                answer.RuleFor(a => a.QuestionId).GreaterThan(0);
                // Ya OptionId ya da TextAnswer dolu olmalı mantığı eklenebilir
            });
        }
    }
}
