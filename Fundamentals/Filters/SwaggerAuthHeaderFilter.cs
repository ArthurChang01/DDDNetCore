using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Fundamentals.Filters
{
    public class SwaggerAuthHeaderFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = (context.ApiDescription.ActionDescriptor as ControllerActionDescriptor).MethodInfo.CustomAttributes;
            var isAuthorized = filterPipeline.Any(o => o.AttributeType == typeof(AuthorizeAttribute));
            var allowAnonymous = filterPipeline.Any(o => o.AttributeType == typeof(AllowAnonymousAttribute));

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<IParameter>();
                operation.Parameters.Add(new NonBodyParameter
                {
                    Name = "Authorization",
                    In = "header",
                    Description = "OAuth Token",
                    Required = true,
                    Type = "string",
                    Default = "Bearer "
                });
            }
        }
    }
}