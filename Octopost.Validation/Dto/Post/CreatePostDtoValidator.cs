namespace Octopost.Validation.Dto.Newsletter
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class CreatePostDtoValidator : AbstractOctopostValidator<CreatePostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 1000);
            this.AddRuleForLocation();
        }
    }
}
