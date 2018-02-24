namespace Octopost.Model.ApiResponse.HTTP200
{
    public interface IOkApiResult : IApiResult
    {
        object Object { get; set; }
    }
}
