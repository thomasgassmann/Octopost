namespace Octopost.Services.Comments
{
    using Microsoft.EntityFrameworkCore;
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.Common;
    using Octopost.Services.Exceptions;
    using Octopost.Services.UoW;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CommentFilterService : ICommentFilterService
    {
        private readonly IQueryService queryService;

        private readonly IPagingValidator pagingValidator;

        public CommentFilterService(IQueryService queryService, IPagingValidator pagingValidator)
        {
            this.queryService = queryService;
            this.pagingValidator = pagingValidator;
        }

        public IEnumerable<CommentDto> GetCommentsForPost(long postId, int page, int pageSize)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            this.ThrowIfPostDoesNotExist(postId);
            var comments = this.QueryCommentDto();
            var queried = comments
                .Where(x => x.PostId == postId)
                .OrderByDescending(x => x.Created)
                .ThenBy(x => x.LocationName);
            var filtered = this.Page(queried, page, pageSize);
            return filtered.ToList();
        }

        public IEnumerable<CommentDto> GetCommentsForPostSince(DateTime date, long postId)
        {
            this.ThrowIfPostDoesNotExist(postId);
            var comments = this.QueryCommentDto();
            var queried = comments
                .Where(x => x.PostId == postId && x.Created >= date)
                .OrderByDescending(x => x.Created)
                .ThenBy(x => x.LocationName);
            return queried.ToList();
        }

        private void ThrowIfPostDoesNotExist(long postId)
        {
            if (!this.queryService.Query<Post>().Any(x => x.Id == postId))
            {
                throw new ApiException(x => x.BadRequestResult((
                    ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, OctopostEntityName.Comment, PropertyName.Comment.PostId, OctopostEntityName.Post),
                    new ErrorDefinition(postId, "The given post does not exist", PropertyName.Comment.PostId))));
            }
        }

        private IQueryable<CommentDto> QueryCommentDto()
        {
            var fetched =
                from comment in this.queryService.Query<Comment>().Include(x => x.LocationName)
                select new CommentDto
                {
                    Text = comment.Text,
                    PostId = comment.PostId,
                    Created = comment.Created,
                    Id = comment.Id,
                    Latitude = comment.Latitude,
                    Longitude = comment.Longitude,
                    LocationName = comment.LocationName.Name
                };
            return fetched;
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
