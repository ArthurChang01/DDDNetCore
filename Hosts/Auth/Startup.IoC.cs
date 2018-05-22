using Autofac;
using Autofac.Extensions.DependencyInjection;
using Fundamentals.Miscs;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Auth
{
    public partial class Startup
    {
        public IServiceProvider ConfigureIoC(IServiceCollection services)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.Populate(services);
            AutofacConfig.RegisterComponent(services, builder, this.BoundedContext);
            this.ApplicationContainer = builder.Build();

            return new AutofacServiceProvider(this.ApplicationContainer);
        }
    }
}