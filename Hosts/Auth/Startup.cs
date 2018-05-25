using Autofac;
using Fundamentals.Middlewares;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.OptionObjects;
using System;
using System.IO;

namespace Auth
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                 .AddEnvironmentVariables()
                 .Build();
        }

        public string BoundedContext => Configuration.GetValue<string>("BOUNDED_CONTEXT") ?? "";
        public IContainer ApplicationContainer { get; private set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //Options
            services.Configure<DbConfig>(this.Configuration.GetSection("Mongo"));
            services.Configure<MQConfig>(Configuration.GetSection("RabbitMQ"));
            services.Configure<AuthTokenConfig>(this.Configuration.GetSection("AuthToken"));

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            app.UseResponseCompression();
            app.UseGlobalException();

            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
            }

            this.ApplySwagger(app);

            app.UseCors("corsPolicy");
            app.UseAuthentication();
            app.UseMvc();

            var busHandle = ConnectRabbitMQ();

            appLifetime.ApplicationStopping.Register(() =>
            {
                busHandle.Stop();
                this.ApplicationContainer.Dispose();
            });
        }

        private BusHandle ConnectRabbitMQ()
        {
            BusHandle busHandle = null;

            var bus = this.ApplicationContainer.Resolve<IBusControl>();
            byte errorCounter = 0;
            while (busHandle == null && errorCounter < 5)
            {
                errorCounter++;
                try
                {
                    busHandle = TaskUtil.Await(() => bus.StartAsync());
                }
                catch (Exception) { }
            }

            return busHandle;
        }
    }
}