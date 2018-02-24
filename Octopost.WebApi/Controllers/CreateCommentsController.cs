namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Comments;

    [Route("api/Posts")]
    public class CreateCommentsController : Controller
    {
        private readonly ICommentCreationService commentCreationService;

        private readonly IApiResultService apiResultService;

        public CreateCommentsController(ICommentCreationService commentCreationService, IApiResultService apiResultService)
        {
            this.commentCreationService = commentCreationService;
            this.apiResultService = apiResultService;
        }

        [HttpPost("{postId}/Comments")]
        public IActionResult CreateComment([FromRoute] long postId, [FromBody] CreateCommentDto createCommentDto)
        {
            var id = this.commentCreationService.CreateComment(postId, createCommentDto);
            return this.apiResultService.Created(OctopostEntityName.Comment, id);
        }
    }
}
