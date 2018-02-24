namespace Octopost.Services.Posts
{
    using Octopost.Model.Dto;
    using System;
    using System.Collections.Generic;

    public interface IPostFilterService
    {
        IEnumerable<PostDto> FilterByTag(string[] tag, int page, int pageSize, int comments = 0);

        IEnumerable<PostDto> FilterByDate(DateTime from, DateTime to, int page, int pageSize, int comments = 0);

        IEnumerable<PostDto> FilterByVotes(int page, int pageSize, int comments = 0);

        IEnumerable<PostDto> FilterByQuery(string query, int page, int pageSize, int comments = 0);

        IEnumerable<PostDto> FilterByLocation(double latitude, double longitude, int page, int pageSize, int comments = 0);
    }
}
