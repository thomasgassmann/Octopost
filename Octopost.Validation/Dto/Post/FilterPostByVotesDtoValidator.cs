namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class FilterPostByVotesDtoValidator : AbstractOctopostValidator<PagedDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPaging();
        }
    }
}
