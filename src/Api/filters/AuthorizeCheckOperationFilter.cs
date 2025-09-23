using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace Api.filters;

public class AuthorizeCheckOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Se a action ou controller tem [Authorize] e não tem [AllowAnonymous]
        var hasAuthorize = (context.MethodInfo.DeclaringType != null &&
                               context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                                   .OfType<AuthorizeAttribute>().Any()) ||
                           context.MethodInfo.GetCustomAttributes(true)
                               .OfType<AuthorizeAttribute>().Any();

        var allowAnonymous = context.MethodInfo.GetCustomAttributes(true)
                                 .OfType<AllowAnonymousAttribute>().Any();

        if (hasAuthorize && !allowAnonymous)
        {
            operation.Security ??= new List<OpenApiSecurityRequirement>();

            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [scheme] = new string[] { }
            });
        }
    }
}
