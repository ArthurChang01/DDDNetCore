using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public partial class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureApiVersion(IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("v"),
                    new HeaderApiVersionReader { HeaderNames = { "api-version" } }
                    );
            });
        }
    }
}