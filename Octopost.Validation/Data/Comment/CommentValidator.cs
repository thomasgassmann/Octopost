namespace Octopost.Validation.Data.Comment
{
    using Octopost.Model.Data;
    using Octopost.Validation.Common;

    public class CommentValidator : AbstractOctopostValidator<Comment>
    {
        protected override void Initalize()
        {
            this.AddRuleForPostText(x => x.Text, 10, 100);
            this.AddRuleForLocation();
        }
    }
}
