namespace Octopost.Model.ApiResponse.HTTP403
{
    using Octopost.Model.Validation;

    public interface IForbiddenApiResult : IApiResult
    {
        long UserId { get; set; }

        OctopostEntityName AccessedEntityType { get; set; }

        long AccessedEntityId { get; set; }
    }
}
