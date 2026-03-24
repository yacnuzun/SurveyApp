using FluentValidation;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Application.Validators
{
    public class QuestionForCreateDtoValidator : AbstractValidator<QuestionForCreateDto>
    {
        public QuestionForCreateDtoValidator()
        {
            RuleFor(q => q.Text)
            .NotEmpty().WithMessage("Soru metni boş olamaz.")
            .MaximumLength(500).WithMessage("Soru metni en fazla 500 karakter olabilir.");

            RuleFor(q => q.AnswerTemplateId)
                .NotNull().WithMessage("Seçimli sorular için cevap şablonu seçilmelidir.")
                .When(q => q.Type != QuestionType.Text);

        }
    }
}
