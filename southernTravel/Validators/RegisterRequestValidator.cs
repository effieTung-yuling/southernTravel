using FluentValidation;
using southernTravel.Model;
namespace southernTravel.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email 是必填的")
                .EmailAddress().WithMessage("Email 格式不正確");

            RuleFor(x => x.PhoneNumber)
                .Matches(@"^\d+$").WithMessage("電話號碼只能包含數字");

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("密碼至少 6 位")
                .Matches(@"[A-Z]").WithMessage("密碼必須包含一個大寫英文字母");

            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("生日必填");
        }
    }
}
