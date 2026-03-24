using FluentValidation;
using SurveyApp.SurveyFollow.Application.Dto_s;

namespace SurveyApp.SurveyFollow.Application.Validators
{
    public class ParticipationForCreateDtoValidator : AbstractValidator<ParticipationForCreateDto>
    {
        public ParticipationForCreateDtoValidator()
        {
            RuleFor(p => p.SurveyId)
            .GreaterThan(0).WithMessage("Geçerli bir anket seçilmelidir.");

            RuleFor(p => p.Answers)
                .NotEmpty().WithMessage("En az bir cevap girilmelidir.");

            RuleForEach(p => p.Answers)
                .SetValidator(new AnswerForCreateDtoValidator());

        }
    }
    public class AnswerForCreateDtoValidator : AbstractValidator<AnswerForCreateDto>
    {
        public AnswerForCreateDtoValidator()
        {
            RuleFor(a => a.QuestionId)
                .GreaterThan(0).WithMessage("Geçerli bir soru seçilmelidir.");

            // OptionId veya TextAnswer'dan biri dolu olmalı
            RuleFor(a => a)
                .Must(a => a.OptionId.HasValue || !string.IsNullOrEmpty(a.TextAnswer))
                .WithMessage("Her soru için bir cevap girilmelidir.");
        }
    }

}
