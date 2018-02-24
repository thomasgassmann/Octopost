namespace Octopost.Model.ApiResponse.HTTP200
{
    using System.Net;

    public class OkApiResult : ApiResultBase, IOkApiResult
    {
        public OkApiResult(object obj) =>
            this.Object = obj;

        public object Object { get; set; }

        public override HttpStatusCode Code => HttpStatusCode.OK;

        public override bool Success => true;

        public override string Message { get; set; } = "OK";

        public override object GetJsonObject()
        {
            if (this.Object != null)
            {
                return this.Object;
            }

            return new
            {
                Success = this.Success,
                Message = "Success",
            };
        }
    }
}
