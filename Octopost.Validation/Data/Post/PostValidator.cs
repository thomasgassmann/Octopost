namespace Octopost.Validation.Data.Post
{
    using Octopost.Model.Data;
    using Octopost.Validation.Common;

    public class PostValidator : AbstractOctopostValidator<Post>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 1000);
            this.AddRuleForPostTopic(x => x.Topic);
            this.AddRuleForLocation();
        }
    }
}
