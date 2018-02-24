namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Votes;
    using System;

    /// <summary>
    /// Contains all end points to vote for posts.
    /// </summary>
    [Route("/api/Posts")]
    public class VotePostController : Controller
    {
        /// <summary>
        /// Contains the service to vote for posts.
        /// </summary>
        private readonly IVoteService voteService;

        /// <summary>
        /// Contains the service to format API results.
        /// </summary>
        private readonly IApiResultService apiResultService;

        /// <summary>
        /// Initializes a new instance of the <see cref="VotePostController"/> class.
        /// </summary>
        /// <param name="voteService">The service to vote for a post.</param>
        /// <param name="apiResultService">The service to format API results.</param>
        public VotePostController(IVoteService voteService, IApiResultService apiResultService)
        {
            this.voteService = voteService;
            this.apiResultService = apiResultService;
        }

        /// <summary>
        /// Votes on a post as specified in the data transfer object.
        /// </summary>
        /// <param name="dto">The object to hold the identifier of the post to vote for.</param>
        /// <returns>Returns a created results or a bad request.</returns>
        [HttpPost("{postId}/Votes")]
        public IActionResult Vote(CreateVoteDto dto)
        {
            var state = (VoteState)Enum.Parse(typeof(VoteState), dto.VoteState);
            var created = this.voteService.Vote(dto.PostId, state);
            return this.apiResultService.Created(OctopostEntityName.Vote, created);
        }
    }
}
