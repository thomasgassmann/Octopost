namespace Octopost.Services.Votes
{
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Validation;
    using Octopost.Services.Exceptions;
    using Octopost.Services.UoW;
    using System.Linq;

    public class VoteService : IVoteService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        public VoteService(IUnitOfWorkFactory unitOfWorkFactory)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
        }

        public long Vote(long postId, VoteState voteState)
        {
            if (!this.PostExists(postId))
            {
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, OctopostEntityName.Post, PropertyName.Post.Id),
                    new ErrorDefinition(postId, "Post does not exist", PropertyName.Post.Id))));
            }

            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var voteRepo = unitOfWork.CreateEntityRepository<Vote>();
                var vote = new Vote
                {
                    PostId = postId,
                    State = voteState
                };
                voteRepo.Create(vote);
                unitOfWork.Save();
                return vote.Id;
            }
        }

        private bool PostExists(long postId)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var postRepository = unitOfWork.CreateEntityRepository<Post>();
                return postRepository.Query().Any(x => x.Id == postId);
            }
        }
    }
}
