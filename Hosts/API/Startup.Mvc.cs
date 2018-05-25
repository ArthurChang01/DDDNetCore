using FluentValidation.AspNetCore;
using Fundamentals.Miscs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API
{
    public partial class Startup
    {
        public void ConfigureMvc(IServiceCollection services)
        {
            var builder = services
                .AddCors(opt =>
                    opt.AddPolicy("corsPolicy", policy => policy
                          .WithOrigins(this.Configuration.GetValue<string>("cors"))
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowAnyOrigin())
                )
                .AddMvcCore(cfg =>
                {
                    cfg.Filters.Add(
                        new AuthorizeFilter(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())
                    );
                })
                .AddAuthorization()
                .AddApiExplorer()
                .AddJsonFormatters(o =>
                {
                    o.Converters.Clear();
                    o.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    o.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                })
                .ConfigureApplicationPartManager(opt =>
                {
                    opt.FeatureProviders.Clear();
                    var featureProvider = new ServiceFeatureProvider(this.BoundedContext);
                    opt.FeatureProviders.Add(featureProvider);
                });

            if (!this.HostEnvironment.IsDevelopment())
                builder
                    .AddFluentValidation(fvc => fvc.RegisterValidatorsFromAssemblyContaining<Startup>());
        }
    }
}