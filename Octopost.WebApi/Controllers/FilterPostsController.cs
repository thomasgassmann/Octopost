namespace Octopost.WebApi.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.Dto;
    using Octopost.Services.ApiResult;
    using Octopost.Services.Comments;
    using Octopost.Services.Posts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Accepts all requests to filter posts.
    /// </summary>
    [Route("api/Posts")]
    public class FilterPostsController : Controller
    {
        /// <summary>
        /// Contains the service to filter posts.
        /// </summary>
        private readonly IPostFilterService postFilterService;

        /// <summary>
        /// Contains the service to return formatted JSON results to the client.
        /// </summary>
        private readonly IApiResultService apiResultService;

        private readonly ICommentFilterService commentFilterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterPostsController"/> class.
        /// </summary>
        /// <param name="postFilterService">The service to filter posts.</param>
        /// <param name="apiResultService">The service for formatted API results.</param>
        public FilterPostsController(IPostFilterService postFilterService, IApiResultService apiResultService, ICommentFilterService commentFilterService)
        {
            this.postFilterService = postFilterService;
            this.apiResultService = apiResultService;
            this.commentFilterService = commentFilterService;
        }

        /// <summary>
        /// Gets all posts by their tags.
        /// </summary>
        /// <param name="dto">The paging and the tags to filter for.</param>
        /// <returns>Returns the filtered posts.</returns>
        [HttpGet("Tags")]
        public IActionResult FilterByTags(FilterPostByTagDto dto)
        {
            var separated = dto.Tags.Split(',').Select(x => x.Trim()).ToArray();
            var result = this.postFilterService.FilterByTag(separated, dto.PageNumber, dto.PageSize, dto.CommentNumber);
            return this.apiResultService.Ok(result);
        }

        /// <summary>
        /// Gets all posts by their vote count.
        /// </summary>
        /// <param name="dto">The paging.</param>
        /// <returns>Returns the filtered posts.</returns>
        [HttpGet("Votes")]
        public IActionResult FilterByVotes(PagedPostDto dto)
        {
            var result = this.postFilterService.FilterByVotes(dto.PageNumber, dto.PageSize, dto.CommentNumber);
            return this.apiResultService.Ok(result);
        }

        [HttpGet("{postId}/Comments/Date")]
        public IActionResult GetComments(long postId, [FromQuery] DateTime since)
        {
            var result = this.commentFilterService.GetCommentsForPostSince(since, postId);
            return this.apiResultService.Ok(result);
        }

        [HttpGet("{postId}/Comments")]
        public IActionResult GetComments(long postId, PagedDto dto)
        {
            var result = this.commentFilterService.GetCommentsForPost(postId, dto.PageNumber, dto.PageSize);
            return this.apiResultService.Ok(result);
        }

        /// <summary>
        /// Filters for the newest posts.
        /// </summary>
        /// <param name="dto">The paging.</param>
        /// <returns>Returns the filtered posts.</returns>
        [HttpGet("Newest")]
        public IActionResult GetNewest(PagedPostDto dto)
        {
            var result = this.postFilterService.FilterByDate(DateTime.MinValue, DateTime.MaxValue, dto.PageNumber, dto.PageSize, dto.CommentNumber);
            return this.apiResultService.Ok(result);
        }

        /// <summary>
        /// Queries the posts by a search query.
        /// </summary>
        /// <param name="dto">The search query and the paging.</param>
        /// <returns>Returns the filtered posts.</returns>
        [HttpGet("Query")]
        public IActionResult Query(FilterPostByQueryDto dto)
        {
            var result = this.postFilterService.FilterByQuery(dto.Query, dto.PageNumber, dto.PageSize, dto.CommentNumber);
            return this.apiResultService.Ok(result);
        }

        /// <summary>
        /// Queries the posts by a date range.
        /// </summary>
        /// <param name="dto">The paging and the start- and end-date.</param>
        /// <returns>Returns the posts in the given date range on the given page.</returns>
        [HttpGet("Date")]
        public IActionResult ByDate(FilterPostByDateDto dto)
        {
            var result = this.postFilterService.FilterByDate(dto.From.Value, dto.To.Value, dto.PageNumber, dto.PageSize, dto.CommentNumber);
            return this.apiResultService.Ok(result);
        }

        /// <summary>
        /// Queries the posts by closeness to the given location.
        /// </summary>
        /// <param name="dto">The location to find close posts for.</param>
        /// <returns>Returns the posts found.</returns>
        [HttpGet("Location")]
        public IActionResult GetClosePosts(FilterPostByLocationDto dto)
        {
            var result = this.postFilterService.FilterByLocation(dto.Latitude, dto.Longitude, dto.PageNumber, dto.PageSize, dto.CommentNumber);
            return this.apiResultService.Ok(result);
        }
    }
}
