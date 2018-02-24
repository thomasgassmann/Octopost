namespace Octopost.Model.ApiResponse.HTTP500
{
    using System;
    using System.Diagnostics;
    using System.Net;

    public class InternalServerErrorApiResult : ApiResultBase, IInternalServerErrorApiResult
    {
        public InternalServerErrorApiResult(Exception ex)
        {
            this.Exception = ex;
        }

        public Exception Exception { get; set; }

        public override HttpStatusCode Code => HttpStatusCode.InternalServerError;

        public override bool Success => false;

        public override string Message { get; set; } = "Something went wrong";

        public override object GetJsonObject()
        {
            return new
            {
                Success = false,
                Message = this.Message,
                Exception = this.Exception
            };
        }
    }
}
