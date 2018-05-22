using Fundamentals.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Auth
{
    public partial class Startup
    {
        public void ConfigureSwagger(IServiceCollection services)
        {
            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = $"{this.BoundedContext} API", Version = "v1" });
            });
            services.ConfigureSwaggerGen(opt =>
            {
                opt.DescribeAllEnumsAsStrings();
                opt.CustomSchemaIds(x => x.FullName);
                opt.OperationFilter<SwaggerAuthHeaderFilter>();
                opt.DocumentFilter<SwaggerPickUpAPIFilter>(this.BoundedContext);
            });
        }

        public void ApplySwagger(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                $"{this.BoundedContext} API v1"));
        }
    }
}