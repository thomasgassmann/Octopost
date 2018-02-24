namespace Octopost.Model.ApiResponse.HTTP400
{
    using Octopost.Model.Validation;

    public class ErrorDefinition
    {
        public ErrorDefinition(object attemptedValue, string errorMessage, PropertyName propertyName)
        {
            this.AttemptedValue = attemptedValue;
            this.Message = errorMessage;
            this.Property = propertyName;
        }

        public object AttemptedValue { get; set; }

        public string Message { get; set; }

        public PropertyName Property { get; set; }
    }
}
