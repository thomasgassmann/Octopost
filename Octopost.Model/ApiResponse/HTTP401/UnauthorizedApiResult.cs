namespace Octopost.Model.ApiResponse.HTTP401
{
    using System.Net;

    public class UnauthorizedApiResult : ApiResultBase, IUnauthorizedApiResult
    {
        public override HttpStatusCode Code => HttpStatusCode.Unauthorized;

        public override bool Success => false;

        public override string Message { get; set; } = "You're not authenticated. Please re-enter your credentials";

        public override object GetJsonObject() => new
        {
            Success = false,
            Message = this.Message
        };
    }
}
