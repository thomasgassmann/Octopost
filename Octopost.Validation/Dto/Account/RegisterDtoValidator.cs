namespace Octopost.Validation.Dto.Account
{
    using FluentValidation;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Validation.Common;
    using System.Linq;

    public class RegisterDtoValidator : AbstractOctopostValidator<RegisterDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForNotNullOrEmpty(x => x.Email, OctopostEntityName.Account, PropertyName.Account.Email);
            this.AddRuleForNotNullOrEmpty(x => x.FirstName, OctopostEntityName.Account, PropertyName.Account.FirstName);
            this.AddRuleForNotNullOrEmpty(x => x.LastName, OctopostEntityName.Account, PropertyName.Account.LastName);
            this.AddRuleForNotNullOrEmpty(x => x.Password, OctopostEntityName.Account, PropertyName.Account.Password);
            this.RuleFor(x => x.Email).EmailAddress()
                .WithErrorCode(ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, OctopostEntityName.Account, PropertyName.Account.Email).Code)
                .WithMessage("Please provide a valid mail address");
            this.AddRuleForMinLength(x => x.Password, 10, OctopostEntityName.Account, PropertyName.Account.Password);
            this.RuleFor(x => x.Password)
                .Must(x => x != null && x.Any(char.IsUpper) && x.Any(char.IsLower) && x.Any(char.IsDigit))
                .WithErrorCode(ErrorCode.Parse(ErrorCodeType.OutOfRange, OctopostEntityName.Account, PropertyName.Account.Password).Code)
                .WithMessage("Password must contain uppercase, lowercase and digit characters");
        }
    }
}
