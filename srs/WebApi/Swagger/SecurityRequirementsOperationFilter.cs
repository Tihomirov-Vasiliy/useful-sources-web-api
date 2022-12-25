using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Swagger
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (!context.MethodInfo.GetCustomAttributes(true).Any(a=>a is AllowAnonymousAttribute)&&
                context.MethodInfo.GetCustomAttributes(true).Any(a=>a is AuthorizeAttribute))
               // && !(context.MethodInfo.DeclaringType?.GetCustomAttributes(true).Any(a=>a is AllowAnonymousAttribute) ?? false))
            {
                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },  new string[] { }
                        }
                    }
                };
            }
        }
    }
}
