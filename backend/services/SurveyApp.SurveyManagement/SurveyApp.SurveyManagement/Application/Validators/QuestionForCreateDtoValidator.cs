using FluentValidation;
using SurveyApp.SurveyManagement.Application.Dto_s;
using SurveyApp.SurveyManagement.Domain.Enums;

namespace SurveyApp.SurveyManagement.Application.Validators
{
    public class QuestionForCreateDtoValidator : AbstractValidator<QuestionForCreateDto>
    {
        public QuestionForCreateDtoValidator()
        {
            RuleFor(q => q.Text).NotEmpty().WithMessage("Soru metni boş olamaz.");
            RuleFor(q => q.Order).GreaterThan(0).WithMessage("Soru sırası 0'dan büyük olmalıdır.");

            // Eğer soru tipi Single veya Multi ise en az 2 seçenek olmalı
            RuleFor(q => q.Options)
                .Must(o => o != null && o.Count >= 2)
                .When(q => q.Type == QuestionType.Single || q.Type == QuestionType.Multi)
                .WithMessage("Çoktan seçmeli sorular en az 2 seçenek içermelidir.");
        }
    }
}
