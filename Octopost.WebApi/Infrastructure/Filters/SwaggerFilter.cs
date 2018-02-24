namespace Octopost.WebApi.Infrastructure.Filters
{
    using Microsoft.AspNetCore.Http;
    using Swashbuckle.Swagger.Model;
    using Swashbuckle.SwaggerGen.Generator;
    using System.Linq;

    public class SwaggerFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.ParameterDescriptions.Any(x => x.Type == typeof(IFormFile)))
            {
                operation.Parameters.Clear();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "file",
                    In = "formData",
                    Description = "Upload file",
                    Required = true,
                    Type = "file"
                });
                operation.Consumes.Add("application/form-data");
            }
        }
    }
}
