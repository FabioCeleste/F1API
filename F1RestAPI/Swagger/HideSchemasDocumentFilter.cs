using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace F1RestAPI.Swagger;
public class HideSchemasDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        var schemasToHide = new[] { "CreateConstructorRequest", "CreateDriverConstructorRequest", "CreateDriverRequest", "CreateUserRequest", "DeleteDriverConstructorRequest", "UpdateConstructorRequest", "UpdateDriverConstructorResponse", "UpdateDriverConstructorRequest", "UpdateDriverRequest" };

        foreach (var schemaName in schemasToHide)
        {
            if (swaggerDoc.Components.Schemas.ContainsKey(schemaName))
            {
                swaggerDoc.Components.Schemas.Remove(schemaName);
            }
        }
    }
}
