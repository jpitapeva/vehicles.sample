using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace Case.WebApi.Swagger
{
    public class ReplaceVersionForCorrectValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (swaggerDoc == null)
                throw new ArgumentNullException(nameof(swaggerDoc));

            var openApiPaths = new OpenApiPaths();

            foreach (var (key, value) in swaggerDoc.Paths)
                openApiPaths.Add(key.Replace("v{version}", swaggerDoc.Info.Version, StringComparison.InvariantCulture), value);

            swaggerDoc.Paths = openApiPaths;
        }
    }
}