namespace Octopost.Services.Common
{
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Validation;
    using Octopost.Services.Exceptions;
    using System.Collections.Generic;
    using System.Linq;

    public class PagingValidator : IPagingValidator
    {
        public void ThrowIfPageOutOfRange(int pageSize, int page)
        {
            var errors = new Dictionary<ErrorCode, ErrorDefinition>();
            if (pageSize < 0)
            {
                errors.Add(
                    ErrorCode.Parse(ErrorCodeType.OutOfRange, OctopostEntityName.Filter, PropertyName.Filter.PageSize),
                    new ErrorDefinition(pageSize, "Page size must be 0 or bigger", PropertyName.Filter.PageSize));
            }

            if (page < 0)
            {
                errors.Add(
                    ErrorCode.Parse(ErrorCodeType.OutOfRange, OctopostEntityName.Filter, PropertyName.Filter.PageNumber),
                    new ErrorDefinition(page, "Page number cannot be negative", PropertyName.Filter.PageNumber));
            }

            if (errors.Any())
            {
                throw new ApiException(x => x.BadRequestResult(errors));
            }
        }
    }
}
