namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Posts;

    /// <summary>
    /// Accepts all requests to create posts.
    /// </summary>
    [Route("api/Posts")]
    public class CreatePostsController : Controller
    {
        /// <summary>
        /// Contains the <see cref="IPostCreationService"/> to create posts.
        /// </summary>
        private readonly IPostCreationService postCreationService;

        /// <summary>
        /// Contains the <see cref="IApiResultService"/> to return formatted JSON results to the client.
        /// </summary>
        private readonly IApiResultService apiResultService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreatePostsController"/> class.
        /// </summary>
        /// <param name="postCreationService">The post creation service to create the posts.</param>
        /// <param name="apiResultService">The service to return results to the client.</param>
        public CreatePostsController(IPostCreationService postCreationService, IApiResultService apiResultService)
        {
            this.postCreationService = postCreationService;
            this.apiResultService = apiResultService;
        }

        /// <summary>
        /// Accepts all requests to create posts.
        /// </summary>
        /// <param name="createPostDto">The data to create the posts from.</param>
        /// <returns>Returns the created result or a bad request.</returns>
        [HttpPost]
        public IActionResult CreatePost([FromBody] CreatePostDto createPostDto)
        {
            var id = this.postCreationService.CreatePost(createPostDto);
            return this.apiResultService.Created(OctopostEntityName.Post, id);
        }
    }
}
