namespace Octopost.Validation.Dto.Post
{
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Validation.Common;
    using System;

    public class FilterPostByDateDtoValidator : AbstractOctopostValidator<FilterPostByDateDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForPaging();
            this.AddRuleForDate(x => x.From, new DateTime(2016, 01, 01), DateTime.Now.AddYears(3), PropertyName.Post.From);
            this.AddRuleForDate(x => x.To, new DateTime(2016, 01, 01), DateTime.Now.AddYears(3), PropertyName.Post.To);
        }
    }
}
