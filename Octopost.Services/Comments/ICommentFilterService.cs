namespace Octopost.Services.Comments
{
    using Octopost.Model.Dto;
    using System;
    using System.Collections.Generic;

    public interface ICommentFilterService
    {
        IEnumerable<CommentDto> GetCommentsForPost(long postId, int page, int pageSize);
        
        IEnumerable<CommentDto> GetCommentsForPostSince(DateTime date, long postId);
    }
}
