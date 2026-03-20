using FluentValidation;
using SurveyApp.IdentityService.Application.Dto_s;
using SurveyApp.IdentityService.Application.Services.Interfaces;
using SurveyApp.Shared.Constant;

namespace SurveyApp.IdentityService.Application.Validators
{
    public class RegisterUserValidator : AbstractValidator<UserForRegisterDto>
    {
        private readonly IUserService _userService;
        public RegisterUserValidator(IUserService _userService)
        {
            RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(3)
            .WithMessage("Kullanıcı adı en az 3 karakter olmalıdır.");

            RuleFor(x => x)
            .MustAsync(async (dto, cancellation) =>
            {
                var user = await _userService.GetExistUser(dto.Email, dto.UserName);
                return !user.Success;
            }).WithMessage(Messages.FailedCustomerProccess);

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta adresi boş bırakılamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Şifre boş bırakılamaz.")
                .MinimumLength(6).WithMessage("Şifre en az 6 karakter olmalıdır.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("Geçersiz rol seçimi.");

        }
    }
}
