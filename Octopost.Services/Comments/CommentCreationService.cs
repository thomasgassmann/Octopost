namespace Octopost.Services.Comments
{
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Extensions;
    using Octopost.Services.Location;
    using Octopost.Services.UoW;

    public class CommentCreationService : ICommentCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly ILocationNameService locationNameService;

        public CommentCreationService(IUnitOfWorkFactory unitOfWorkFactory, ILocationNameService locationNameService)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.locationNameService = locationNameService;
        }

        public long CreateComment(long postId, CreateCommentDto createCommentDto)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                var comment = createCommentDto.MapTo<Comment>();
                comment.PostId = postId;
                var commentRepository = unitOfWork.CreateEntityRepository<Comment>();
                comment.LocationNameId = this.locationNameService.NameLocation(createCommentDto);
                commentRepository.Create(comment);
                unitOfWork.Save();
                return comment.Id;
            }
        }
    }
}
