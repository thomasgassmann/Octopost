namespace Octopost.Model.ApiResponse.HTTP500
{
    using System;

    public interface IInternalServerErrorApiResult : IApiResult
    {
        Exception Exception { get; set; }
    }
}
