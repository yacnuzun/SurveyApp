using FluentValidation;
using SurveyApp.IdentityService.Application.Dto_s;

namespace SurveyApp.IdentityService.Application.Validators
{
    public class ClaimValidator : AbstractValidator<ClaimDto>
    {
        public ClaimValidator()
        {
            RuleFor(c => c.Role).IsInEnum();

        }
    }
}
