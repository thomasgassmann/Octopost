namespace Octopost.Services.Posts
{
    using Octopost.Model.Data;
    using Octopost.Services.UoW;
    using System.Linq;

    public class VoteCountService : IVoteCountService
    {
        private readonly IQueryService queryService;

        public VoteCountService(IQueryService queryService)
        {
            this.queryService = queryService;
        }

        public long CountVotes(long postId)
        {
            var query = this.queryService.Query<Vote>();
            var votes = query.Where(x => x.PostId == postId).ToArray();
            var voteCount = votes.Aggregate(0, (current, vote) => (int)vote.State);
            return voteCount;
        }
    }
}
