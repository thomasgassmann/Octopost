namespace Octopost.Model.ApiResponse
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;

    public abstract class ApiResultBase : IApiResult
    {
        public abstract HttpStatusCode Code { get; }

        public abstract bool Success { get; }

        public abstract string Message { get; set; }

        public abstract object GetJsonObject();

        public virtual IActionResult GetResultObject()
        {
            var returnJson = this.GetJsonObject();
            var result = new JsonResult(returnJson)
            {
                StatusCode = (int)this.Code
            };
            result.ContentType = "application/json";
            return result;
        }
    }
}
