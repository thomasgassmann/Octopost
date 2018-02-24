namespace Octopost.Validation.Dto.File
{
    using FluentValidation;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Validation.Common;
    using System;

    public class CreateLinkedFileDtoValidator : AbstractOctopostValidator<CreateLinkedFileDto>
    {
        protected override void Initalize()
        {
            this.RuleFor(x => x.Link)
                .Must(x => !string.IsNullOrEmpty(x) && Uri.TryCreate(x, UriKind.Absolute, out _))
                .WithErrorCode(ErrorCode.Parse(
                    ErrorCodeType.PropertyInvalidData,
                    OctopostEntityName.File,
                    PropertyName.File.Link).Code)
                .WithMessage("Please enter a valid URI");
        }
    }
}
