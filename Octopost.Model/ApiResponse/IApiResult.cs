namespace Octopost.Model.ApiResponse
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    public interface IApiResult
    {
        HttpStatusCode Code { get; }

        bool Success { get; }

        string Message { get; set; }

        IActionResult GetResultObject();
    }
}
