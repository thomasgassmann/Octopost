namespace Octopost.Services.Posts
{
    using Microsoft.EntityFrameworkCore;
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Dto;
    using Octopost.Model.Validation;
    using Octopost.Services.Comments;
    using Octopost.Services.Common;
    using Octopost.Services.Exceptions;
    using Octopost.Services.Files;
    using Octopost.Services.Tagging;
    using Octopost.Services.UoW;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class PostFilterService : IPostFilterService
    {
        private readonly IQueryService queryService;

        private readonly IVoteCountService voteCountService;

        private readonly IPostTaggingService postTaggingService;

        private readonly IPagingValidator pagingValidator;

        private readonly ICommentFilterService commentFilterService;

        private readonly IFileService fileService;

        public PostFilterService(
            IQueryService queryService,
            IVoteCountService voteCountService,
            IPostTaggingService postTaggingService,
            IPagingValidator pagingValidator,
            ICommentFilterService commentFilterService,
            IFileService fileService)
        {
            this.queryService = queryService;
            this.voteCountService = voteCountService;
            this.postTaggingService = postTaggingService;
            this.pagingValidator = pagingValidator;
            this.commentFilterService = commentFilterService;
            this.fileService = fileService;
        }

        public IEnumerable<PostDto> FilterByDate(DateTime from, DateTime to, int page, int pageSize, int comments = 0)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            var posts = this.QueryPostDto();
            var queried = posts
                .Where(x => x.Created >= from && x.Created <= to)
                .OrderByDescending(x => x.Created)
                .ThenByDescending(x => x.VoteCount);
            var postsPage = this.Page(queried, page, pageSize).ToList();
            this.AddMetadata(postsPage, comments);
            return postsPage;
        }

        public IEnumerable<PostDto> FilterByTag(string[] tags, int page, int pageSize, int comments = 0)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            var classes = this.postTaggingService.GetTags();
            var notFound = new List<string>();
            foreach (var tag in tags)
            {
                if (!classes.Values.Contains(tag))
                {
                    notFound.Add(tag);
                }
            }

            if (notFound.Any())
            {
                var attemptedValues = string.Join(", ", tags);
                var notFoundValues = string.Join(", ", notFound);
                throw new ApiException(x => x.BadRequestResult(
                    (ErrorCode.Parse(ErrorCodeType.InvalidReferenceId, OctopostEntityName.Tag, PropertyName.Tag.TagName, OctopostEntityName.Tag),
                    new ErrorDefinition(attemptedValues, $"The following tags do not exist: {notFoundValues}", PropertyName.Tag.TagName))));
            }

            var posts = this.QueryPostDto()
                .Where(x => tags.Contains(x.Topic))
                .OrderByDescending(x => x.VoteCount)
                .ThenByDescending(x => x.Created);
            var postsPage = this.Page(posts, page, pageSize).ToList();
            this.AddMetadata(postsPage, comments);
            return postsPage;
        }

        public IEnumerable<PostDto> FilterByVotes(int page, int pageSize, int comments = 0)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            var fetched = this.QueryPostDto()
                .OrderByDescending(x => x.VoteCount)
                .ThenByDescending(x => x.Created);
            var postsPage = this.Page(fetched, page, pageSize).ToList();
            this.AddMetadata(postsPage, comments);
            return postsPage;
        }

        public IEnumerable<PostDto> FilterByQuery(string query, int page, int pageSize, int comments = 0)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            var fetched = this.QueryPostDto(query)
                .OrderByDescending(x => x.VoteCount)
                .ThenByDescending(x => x.Created);
            var postsPage = this.Page(fetched, page, pageSize).ToList();
            this.AddMetadata(postsPage, comments);
            return postsPage;
        }

        public IEnumerable<PostDto> FilterByLocation(double latitude, double longitude, int page, int pageSize, int comments = 0)
        {
            this.pagingValidator.ThrowIfPageOutOfRange(pageSize, page);
            var fetched = this.QueryPostDto()
                .Select(x => new
                {
                    Difference = Math.Sqrt(Math.Pow(Math.Abs(x.Longitude - longitude), 2) * Math.Pow(Math.Abs(x.Latitude - latitude), 2)),
                    Post = x
                })
                .OrderBy(x => x.Difference)
                .ThenByDescending(x => x.Post.VoteCount)
                .ThenByDescending(x => x.Post.Created)
                .Select(x => x.Post);
            var postsPage = this.Page(fetched, page, pageSize).ToList();
            this.AddMetadata(postsPage, comments);
            return postsPage;
        }

        private void AddMetadata(IList<PostDto> posts, int commentAmount)
        {
            this.AddComments(posts, commentAmount);
            this.AddFile(posts);
        }

        private void AddFile(IList<PostDto> posts)
        {
            foreach (var post in posts)
            {
                post.File = this.fileService.GetFileInfoForPost(post.Id);
            }
        }

        private void AddComments(IList<PostDto> posts, int amount)
        {
            foreach (var post in posts)
            {
                if (amount <= 0)
                {
                    post.Comments = new CommentDto[0];
                }
                else
                {
                    post.Comments = this.commentFilterService.GetCommentsForPost(post.Id, 0, amount).ToArray();
                }
            }
        }

        private IQueryable<PostDto> QueryPostDto(string postFilterText = "")
        {
            var fetched =
                from post in this.queryService.Query<Post>().Include(x => x.LocationName)
                join vote in this.queryService.Query<Vote>() on post.Id equals vote.PostId into posts
                from postWithVote in posts.DefaultIfEmpty(new Vote { State = VoteState.Neutral })
                where postFilterText.Length != 0 ? post.Text.Contains(postFilterText) : true
                group postWithVote by new { post.Id, post.Text, post.Topic, post.Created, post.Longitude, post.Latitude, post.LocationName } into grouped
                select new PostDto
                {
                    Created = grouped.Key.Created,
                    Text = grouped.Key.Text,
                    Id = grouped.Key.Id,
                    Topic = grouped.Key.Topic,
                    VoteCount = grouped.Sum(x => (int)x.State),
                    Latitude = grouped.Key.Latitude,
                    Longitude = grouped.Key.Longitude,
                    LocationName = grouped.Key.LocationName.Name
                };
            return fetched;
        }

        private IQueryable<T> Page<T>(IQueryable<T> queryable, int page, int pageSize) =>
            queryable.Skip(page * pageSize).Take(pageSize);
    }
}
