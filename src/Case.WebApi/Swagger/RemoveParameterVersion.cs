using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Case.WebApi.Swagger
{
    public class RemoveParameterVersion : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var apiVersionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "api-version");
            if (apiVersionParameter != null)
                operation.Parameters.Remove(apiVersionParameter);

            var versionParameter = operation.Parameters.FirstOrDefault(p => p.Name == "version");
            if (versionParameter != null)
                operation.Parameters.Remove(versionParameter);
        }
    }
}