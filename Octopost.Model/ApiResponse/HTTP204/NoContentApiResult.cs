namespace Octopost.Model.ApiResponse.HTTP204
{
    using System.Net;

    public class NoContentApiResult : ApiResultBase, INoContentApiResult
    {
        public override HttpStatusCode Code => HttpStatusCode.NoContent;

        public override bool Success => true;

        public override string Message { get; set; }

        public override object GetJsonObject() => new
        {
            Success = true,
            Message = "No Content",
        };
    }
}
