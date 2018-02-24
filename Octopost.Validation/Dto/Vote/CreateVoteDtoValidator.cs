namespace Octopost.Validation.Dto.Vote
{
    using Octopost.Model.Dto;
    using Octopost.Validation.Common;

    public class CreateVoteDtoValidator : AbstractOctopostValidator<CreateVoteDto>
    {
        protected override void Initalize()
        {
            this.AddRuleForVoteStateString(x => x.VoteState);
        }
    }
}
