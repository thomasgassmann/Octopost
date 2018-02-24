namespace Octopost.Model.ApiResponse.HTTP404
{
    using Octopost.Model.Validation;

    public interface INotFoundApiResult : IApiResult
    {
        OctopostEntityName AccessedEntityType { get; set; }

        long AccessedEntityId { get; set; }
    }
}
