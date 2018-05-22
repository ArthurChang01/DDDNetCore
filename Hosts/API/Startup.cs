using Autofac;
using Fundamentals.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using SharedKernel.OptionObjects;
using System;
using System.IO;

namespace API
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostEnvironment)
        {
            Configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                 .AddEnvironmentVariables()
                 .Build();
            HostEnvironment = hostEnvironment;
        }

        public string BoundedContext => Configuration.GetValue<string>("BOUNDED_CONTEXT");
        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment HostEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Options
            services.Configure<DbConfig>(Configuration.GetSection("Mongo"));
            services.Configure<MQConfig>(Configuration.GetSection("RabbitMQ"));
            services.Configure<AuthTokenConfig>(this.Configuration.GetSection("AuthToken"));

            //Serilog
            services.AddLogging(builder => builder.AddSerilog(dispose: true));

            //Mediator
            this.ConfigureMediatR(services);

            //Mvc
            this.ConfigureMvc(services);

            //Auth
            this.ConfigureAuth(services);

            //ApiVersion
            this.ConfigureApiVersion(services);

            //Response compression setting
            this.ConfigureResponse(services);

            //Swagger
            this.ConfigureSwagger(services);

            //IoC
            return this.ConfigureIoC(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();

            app.UseResponseCompression();
            app.UseHttpLog();
            app.UseGlobalException();

            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
            }

            this.ApplySwagger(app);
            app.UseCors("corsPolicy");
            app.UseAuthentication();
            app.UseMvc();

            appLifetime.ApplicationStopping.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}