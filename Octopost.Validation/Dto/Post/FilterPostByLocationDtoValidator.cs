namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class FilterPostByLocationDtoValidator : AbstractOctopostValidator<FilterPostByLocationDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForLocation();
            this.AddRuleForPaging();
        }
    }
}
