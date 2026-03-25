using FluentValidation;
using SurveyApp.SurveyManagement.Application.Dto_s;

namespace SurveyApp.SurveyManagement.Application.Validators
{
    public class SurveyForCreateDtoValidator : AbstractValidator<SurveyForCreateDto>
    {
        public SurveyForCreateDtoValidator()
        {
            RuleFor(s => s.Title)
            .NotEmpty().WithMessage("Anket başlığı boş olamaz.")
            .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(s => s.StartDate.Date)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Başlangıç tarihi bugünden eski olamaz.");

            RuleFor(s => s.EndDate.Date)
                .GreaterThan(s => s.StartDate.Date)
                .WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

            RuleFor(s => s.QuestionIds)
                .NotEmpty().WithMessage("Anket en az bir soru içermelidir.");

            RuleFor(s => s.AssignedUserIds)
                .NotEmpty().WithMessage("Ankete en az bir kullanıcı atanmalıdır.");

        }
    }
    public class AnswerTemplateForCreateDtoValidator : AbstractValidator<AnswerTemplateForCreateDto>
    {
        public AnswerTemplateForCreateDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Şablon adı boş olamaz.")
                .MaximumLength(100).WithMessage("Şablon adı en fazla 100 karakter olabilir.");

            RuleFor(x => x.Options)
                .NotEmpty().WithMessage("En az 2 seçenek girilmelidir.")
                .Must(o => o.Count >= 2 && o.Count <= 4)
                .WithMessage("Şablon 2 ile 4 arasında seçenek içermelidir.");

            RuleForEach(x => x.Options).ChildRules(o =>
            {
                o.RuleFor(x => x.Text)
                    .NotEmpty().WithMessage("Seçenek metni boş olamaz.")
                    .MaximumLength(200).WithMessage("Seçenek en fazla 200 karakter olabilir.");
            });
        }
    }

}
