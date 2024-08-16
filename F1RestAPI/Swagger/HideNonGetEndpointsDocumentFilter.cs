using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace F1RestAPI.Swagger;
public class HideNonGetEndpointsDocumentFilter : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var path in swaggerDoc.Paths.ToList())
        {
            var operations = path.Value.Operations;

            if (!operations.ContainsKey(OperationType.Get))
            {
                swaggerDoc.Paths.Remove(path.Key);
            }
            else
            {
                foreach (var operationType in operations.Keys.ToList())
                {
                    if (operationType != OperationType.Get)
                    {
                        operations.Remove(operationType);
                    }
                }
            }
        }
    }
}