namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class PagedPostDtoValidator : AbstractOctopostValidator<PagedPostDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPaging();
            this.AddRuleForCommentAmount();
        }
    }
}
