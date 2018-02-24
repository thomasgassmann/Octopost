namespace Octopost.Services.Posts
{
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Extensions;
    using Octopost.Model.Settings;
    using Octopost.Model.Validation;
    using Octopost.Services.Exceptions;
    using Octopost.Services.Location;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using System.Linq;

    public class PostCreationService : IPostCreationService
    {
        private readonly IUnitOfWorkFactory unitOfWorkFactory;

        private readonly IPostTaggingService postTaggingService;

        private readonly ILocationNameService locationNameService;

        private readonly OctopostSettings settings;

        public PostCreationService(
            IUnitOfWorkFactory unitOfWorkFactory,
            IPostTaggingService postTaggingService,
            ILocationNameService locationNameService,
            OctopostSettings settings)
        {
            this.unitOfWorkFactory = unitOfWorkFactory;
            this.postTaggingService = postTaggingService;
            this.locationNameService = locationNameService;
            this.settings = settings;
        }

        public long CreatePost(CreatePostDto createPostDto)
        {
            using (var unitOfWork = this.unitOfWorkFactory.CreateUnitOfWork())
            {
                if (createPostDto.FileId.HasValue)
                {
                    var fileRepository = unitOfWork.CreateEntityRepository<File>();
                    var file = fileRepository.Query().FirstOrDefault(x => x.Id == createPostDto.FileId.Value);
                    if (file == null)
                    {
                        throw new ApiException(x => x.BadRequestResult(
                            (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, OctopostEntityName.Post, PropertyName.Post.FileId, OctopostEntityName.File),
                            new ErrorDefinition(createPostDto.FileId, "The file was not found", PropertyName.Post.FileId))));
                    }
                }

                var repository = unitOfWork.CreateEntityRepository<Post>();
                var post = createPostDto.MapTo<Post>();
                post.Topic = this.PredictTopic(post.Text);
                var namedLocationId = this.locationNameService.NameLocation(post);
                post.LocationNameId = namedLocationId;
                repository.Create(post);
                unitOfWork.Save();
                return post.Id;
            }
        }

        private string PredictTopic(string text) =>
            this.postTaggingService.PredictTag(text);
    }
}
