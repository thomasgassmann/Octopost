namespace Octopost.Model.ApiResponse.HTTP400
{
    using Octopost.Model.Validation;
    using System.Collections.Generic;

    public interface IBadRequestApiResult : IApiResult
    {
        IDictionary<ErrorCode, ErrorDefinition> Errors { get; set; }
    }
}
