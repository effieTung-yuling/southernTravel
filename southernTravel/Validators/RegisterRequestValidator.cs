using FluentValidation;
using southernTravel.DTOs;
namespace southernTravel.Validators
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("姓名必填");
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email 是必填的")
                .EmailAddress().WithMessage("Email 格式不正確");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("電話必填")
                .MinimumLength(9).WithMessage("密碼至少 9 位")
                .Matches(@"^\d+$").WithMessage("電話號碼只能包含數字");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("密碼必填")
                .MinimumLength(6).WithMessage("密碼至少 6 位")
                .Matches(@"[A-Z]").WithMessage("密碼必須包含一個大寫英文字母");
            RuleFor(x => x.Birthday)
                .NotEmpty().WithMessage("生日必填")
                .Must(b =>
                {
                    // 先嘗試 parse
                    if (DateTime.TryParse(b, out var date))
                    {
                        return date <= DateTime.Today; // 生日不能大於今天
                    }
                    return false; // parse 失敗也不通過
                })
                .WithMessage("生日格式錯誤或生日不能大於今天");
        }
    }
}
