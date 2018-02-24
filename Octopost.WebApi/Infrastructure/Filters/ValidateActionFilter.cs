namespace Octopost.WebApi.Infrastructure.Filters
{
    using FluentValidation;
    using FluentValidation.Results;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Octopost.Services.ApiResult;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ValidateActionFilter : IActionFilter
    {
        private readonly IServiceProvider provider;

        private readonly IApiResultService apiResultService;

        public ValidateActionFilter(IServiceProvider provider, IApiResultService apiResultService)
        {
            this.provider = provider;
            this.apiResultService = apiResultService;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var validationList = new List<ValidationResult>();
            foreach (var actionArgument in context.ActionArguments)
            {
                var type = typeof(IValidator<>).MakeGenericType(actionArgument.Value.GetType());
                if (this.provider.GetService(type) is IValidator validator)
                {
                    var result = validator.Validate(actionArgument.Value);
                    if (!result.IsValid)
                    {
                        validationList.Add(result);
                    }
                }
            }

            if (validationList.Any())
            {
                context.Result = this.apiResultService.BadRequest(validationList);
            }
        }
    }
}
