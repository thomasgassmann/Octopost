namespace Octopost.Services.BusinessRules
{
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Data;
    using Octopost.Model.Validation;
    using Octopost.Services.Exceptions;
    using System.Collections.Generic;
    using System.Linq;

    public class LinkedFileBusinessRule : BusinessRuleBase<File>
    {
        public override void PreSave(IList<File> added, IList<File> updated, IList<File> removed)
        {
            foreach (var item in added.Concat(updated))
            {
                if (item.Link != null)
                {
                    if (item.Data != null)
                    {
                        throw new ApiException(result => result.BadRequestResult(
                            (ErrorCode.Parse(ErrorCodeType.PropertyInvalidData, OctopostEntityName.File, PropertyName.File.Link),
                            new ErrorDefinition(item.Link, "Cannot create linked file with data", PropertyName.File.Id))));
                    }

                    item.Data = new byte[] { };
                }
            }
        }
    }
}
