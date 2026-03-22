using FluentValidation;
using SurveyApp.SurveyManagement.Application.Dto_s;

namespace SurveyApp.SurveyManagement.Application.Validators
{
    public class SurveyForCreateDtoValidator : AbstractValidator<SurveyForCreateDto>
    {
        public SurveyForCreateDtoValidator()
        {
            RuleFor(s => s.Title).NotEmpty().WithMessage("Anket başlığı boş olamaz.")
                                 .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(s => s.StartDate.Date)
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Başlangıç tarihi bugünden eski olamaz.");

            RuleFor(s => s.EndDate.Date)
                .GreaterThan(s => s.StartDate.Date)
                .WithMessage("Bitiş tarihi başlangıç tarihinden sonra olmalıdır.");

            RuleFor(s => s.Questions).NotEmpty().WithMessage("Anket en az bir soru içermelidir.");

            // İç içe (Nested) kurallar: Soruları da doğrula
            RuleForEach(s => s.Questions).SetValidator(new QuestionForCreateDtoValidator());
        }
    }
}
