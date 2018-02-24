namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Tagging;

    /// <summary>
    /// Accepts all requests for the Tags endpoint.
    /// </summary>
    [Route("api/[controller]")]
    public class TagsController : Controller
    {
        /// <summary>
        /// Contains the service to tag posts and get all classes.
        /// </summary>
        private readonly IPostTaggingService postTaggingService;

        /// <summary>
        /// Contains the service to return formatted JSON results to the client.
        /// </summary>
        private readonly IApiResultService apiResultService;

        /// <summary>
        /// Initializes a new instance of the <see cref="TagsController"/> class.
        /// </summary>
        /// <param name="postTaggingService">The service used to tag a post.</param>
        /// <param name="apiResultService">The service to format API results.</param>
        public TagsController(IPostTaggingService postTaggingService, IApiResultService apiResultService)
        {
            this.apiResultService = apiResultService;
            this.postTaggingService = postTaggingService;
        }

        /// <summary>
        /// Gets all classes possible for classification.
        /// </summary>
        /// <returns>All classes.</returns>
        [HttpGet]
        public IActionResult GetTags()
        {
            var tags = this.postTaggingService.GetTags();
            return this.apiResultService.Ok(tags);
        }

        /// <summary>
        /// Predicts a class for a given text.
        /// </summary>
        /// <param name="text">The text for predict the class or tag for.</param>
        /// <returns>Returns the class identifier.</returns>
        [HttpPost]
        public IActionResult PredictTag([FromQuery]string text)
        {
            var prediction = this.postTaggingService.PredictTag(text);
            return this.apiResultService.Ok(prediction);
        }
    }
}
