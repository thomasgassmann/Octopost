namespace Octopost.Services.BusinessRules
{
    using FluentValidation;
    using Octopost.Services.BusinessRules.Interfaces;
    using Octopost.Services.Exceptions;
    using Octopost.Services.UoW;
    using System.Collections.Generic;

    public class ValidationBusinessRule : IBusinessRuleBase
    {
        private readonly IValidatorFactory validatorFactory;

        public ValidationBusinessRule(IValidatorFactory validatorFactory) =>
            this.validatorFactory = validatorFactory;

        public void PostSave(IUnitOfWork unitOfWork)
        {
        }

        public void PreSave(IList<object> added, IList<object> updated, IList<object> removed)
        {
            foreach (var item in added)
            {
                this.ValidateObjectAndThrow(item);
            }

            foreach (var item in updated)
            {
                this.ValidateObjectAndThrow(item);
            }
        }

        private void ValidateObjectAndThrow(object obj)
        {
            var validator = this.validatorFactory.GetValidator(obj.GetType());
            if (validator == null)
            {
                return;
            }

            var validationResult = validator.Validate(obj);
            if (!validationResult.IsValid)
            {
                throw new ApiException(x => x.BadRequestResult(validationResult));
            }
        }
    }
}
