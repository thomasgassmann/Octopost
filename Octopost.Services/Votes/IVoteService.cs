namespace Octopost.Services.Votes
{
    using Octopost.Model.Data;

    public interface IVoteService
    {
        long Vote(long postId, VoteState voteState);
    }
}
