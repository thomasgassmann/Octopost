namespace Octopost.Services.Posts
{
    public interface IVoteCountService
    {
        long CountVotes(long postId);
    }
}
