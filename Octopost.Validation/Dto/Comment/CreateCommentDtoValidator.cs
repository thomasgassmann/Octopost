namespace Octopost.Validation.Dto.Comment
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class CreateCommentDtoValidator : AbstractOctopostValidator<CreateCommentDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 100);
            this.AddRuleForLocation();
        }
    }
}
