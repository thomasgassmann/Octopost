namespace Octopost.Model.ApiResponse.HTTP201
{
    using Octopost.Model.Validation;

    public interface ICreatedApiResult : IApiResult
    {
        OctopostEntityName Entity { get; set; }

        long CreatedId { get; set; }
    }
}
