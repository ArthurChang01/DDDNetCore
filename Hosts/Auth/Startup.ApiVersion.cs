using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace Auth
{
    public partial class Startup
    {
        public void ConfigureApiVersion(IServiceCollection services)
        {
            //ApiVersion
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("v"), //svc?v=1.0
                    new HeaderApiVersionReader() { HeaderNames = { "api-version" } });
            });
        }
    }
}