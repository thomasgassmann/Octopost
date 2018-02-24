namespace Octopost.Model.ApiResponse.HTTP403
{
    using Octopost.Model.Validation;
    using System.Net;

    public class ForbiddenApiResult : ApiResultBase, IForbiddenApiResult
    {
        public ForbiddenApiResult(long userId, OctopostEntityName accessedEntity, long accessedEntityId)
        {
            this.UserId = userId;
            this.AccessedEntityType = accessedEntity;
            this.AccessedEntityId = accessedEntityId;
        }

        public long UserId { get; set; }

        public OctopostEntityName AccessedEntityType { get; set; }

        public long AccessedEntityId { get; set; }

        public override HttpStatusCode Code => HttpStatusCode.Forbidden;

        public override bool Success => false;

        public override string Message { get; set; }

        public override object GetJsonObject() => new
        {
            Success = false,
            Message = $"You're not allowed to access this {this.AccessedEntityType.ToString()}",
            AccessedEntityType = this.AccessedEntityType.ToString(),
            AccessedEntityId = this.AccessedEntityId
        };
    }
}
