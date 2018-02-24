namespace Octopost.Model.ApiResponse.HTTP201
{
    using Octopost.Model.Validation;
    using System.Net;

    public class CreatedApiResult : ApiResultBase, ICreatedApiResult
    {
        public CreatedApiResult(OctopostEntityName createdEntity, long createdId)
        {
            this.Entity = createdEntity;
            this.CreatedId = createdId;
        }

        public override string Message { get; set; }

        public override HttpStatusCode Code => HttpStatusCode.Created;

        public override bool Success => true;

        public OctopostEntityName Entity { get; set; }

        public long CreatedId { get; set; }

        public override object GetJsonObject() => new
        {
            Success = true,
            Message = $"{this.Entity.ToString()} created",
            Entity = this.Entity.ToString(),
            CreatedId = this.CreatedId,
        };
    }
}
