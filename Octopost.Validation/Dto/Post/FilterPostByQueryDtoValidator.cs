namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class FilterPostByQueryDtoValidator : AbstractOctopostValidator<FilterPostByQueryDto>
    {
        public FilterPostByQueryDtoValidator()
        {
        }

        protected override void Initalize()
        {
            this.AddRuleForPaging();
            this.AddRuleForQueryText(x => x.Query);
        }
    }
}
