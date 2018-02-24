namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Validation.Common;

    public class FilterPostByTagDtoValidator : AbstractOctopostValidator<FilterPostByTagDto>
    {
        public FilterPostByTagDtoValidator()
        {
        }

        protected override void Initalize()
        {
            this.AddRuleForNotNullOrEmpty(x => x.Tags, OctopostEntityName.Post, PropertyName.Post.Topic);
            this.AddRuleForPaging();
        }
    }
}
