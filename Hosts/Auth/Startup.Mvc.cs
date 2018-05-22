using Fundamentals.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auth
{
    public partial class Startup
    {
        public void ConfigureMvc(IServiceCollection services)
        {
            //Mvc
            services
                .AddCors(opt =>
                {
                    opt.AddPolicy("corsPolicy", policy => policy
                    .WithOrigins(this.Configuration.GetValue<string>("cors"))
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowAnyOrigin());
                })
                .AddMvcCore()
                .AddAuthorization()
                .AddApiExplorer()
                .AddJsonFormatters()
                .AddMvcOptions(o => o.Filters.Add(typeof(DTOValidateFilter)));
        }
    }
}