namespace Octopost.Model.ApiResponse.HTTP404
{
    using Octopost.Model.Validation;
    using System;
    using System.Net;

    public class NotFoundApiResult : ApiResultBase, INotFoundApiResult
    {
        public NotFoundApiResult(OctopostEntityName accessedEntity, long entityId)
        {
            this.AccessedEntityType = accessedEntity;
            this.AccessedEntityId = entityId;
            this.Message = $"{this.AccessedEntityType.ToString()} does not exist. The owner may have deleted it";
        }

        public OctopostEntityName AccessedEntityType { get; set; }

        public long AccessedEntityId { get; set; }

        public override HttpStatusCode Code => HttpStatusCode.NotFound;

        public override bool Success => false;

        public override string Message { get; set; }

        public override object GetJsonObject() => new
        {
            Success = false,
            Message = this.Message,
            AccessedEntityType = this.AccessedEntityType.ToString(),
            AccessedEntityId = this.AccessedEntityId
        };
    }
}
