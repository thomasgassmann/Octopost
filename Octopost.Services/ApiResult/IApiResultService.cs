namespace Octopost.Services.ApiResult
{
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Octopost.Model.ApiResponse;
    using Octopost.Model.ApiResponse.HTTP400;
    using Octopost.Model.Validation;
    using System;
    using System.Collections.Generic;

    public interface IApiResultService
    {
        IApiResult BadRequestResult(IdentityResult identityResult, object attemptedValue, PropertyName propertyName);

        IApiResult NoContentResult();

        IApiResult OkResult(object obj);
        
        IApiResult OkResult();

        IApiResult CreatedResult(OctopostEntityName entity, long id);

        IApiResult BadRequestResult(IDictionary<ErrorCode, ErrorDefinition> errors);

        IActionResult BadRequest(params (ErrorCode code, ErrorDefinition definition)[] errors);

        IApiResult BadRequestResult(params (ErrorCode code, ErrorDefinition definition)[] errors);

        IApiResult BadRequestResult(ValidationResult validationResult);

        IApiResult BadRequestResult(IEnumerable<ValidationResult> validationResult);

        IApiResult UnauthorizedResult();

        IApiResult ForbiddenResult(OctopostEntityName accessedEntity, long accessedEntityId);

        IApiResult NotFoundResult(OctopostEntityName accessedEntity, long accessedEntityId);

        IApiResult InternalServerErrorResult(Exception ex);

        IActionResult NoContent();

        IActionResult Ok(object obj);

        IActionResult Ok();

        IActionResult Created(OctopostEntityName entity, long id);

        IActionResult BadRequest(IDictionary<ErrorCode, ErrorDefinition> errors);

        IActionResult BadRequest(ValidationResult validationResult);

        IActionResult BadRequest(IEnumerable<ValidationResult> validationResult);

        IActionResult Unauthorized();

        IActionResult Forbidden(OctopostEntityName accessedEntity, long accessedEntityId);

        IActionResult NotFound(OctopostEntityName accessedEntity, long accessedEntityId);

        IActionResult InternalServerError(Exception ex);
    }
}
