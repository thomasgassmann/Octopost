namespace Octopost.Model.ApiResponse.HTTP400
{
    using Octopost.Model.Validation;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class BadRequestApiResult : ApiResultBase, IBadRequestApiResult
    {
        public BadRequestApiResult(IDictionary<ErrorCode, ErrorDefinition> errors) =>
            this.Errors = errors;

        public override string Message { get; set; }

        public override HttpStatusCode Code => HttpStatusCode.BadRequest;

        public override bool Success => false;

        public IDictionary<ErrorCode, ErrorDefinition> Errors { get; set; }

        public override object GetJsonObject() => new
        {
            Success = false,
            Message = string.Join(
                ". ",
                this.Errors.Values.Select(x => x.Message).ToArray()),
            Errors = this.Errors.ToDictionary(
                x => x.Key.Code, 
                x => new
                {
                    x.Value.AttemptedValue,
                    x.Value.Message,
                    Property = x.Value.Property?.Name
                })
        };
    }
}
