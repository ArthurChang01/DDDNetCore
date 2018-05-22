using Autofac;
using Autofac.Core.Activators.Reflection;
using MakeUps;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Orders;
using RabbitMQ.Client;
using SharedKernel.Interfaces;
using SharedKernel.OptionObjects;
using System;
using System.Linq;

namespace Fundamentals.Miscs
{
    public class AutofacConfig
    {
        private static IConst[] assemblies;

        public static ContainerBuilder RegisterComponent(IServiceCollection services, ContainerBuilder builder, string boundedContext)
        {
            assemblies = new IConst[] { new OrderConst(services), new MakeUpConst(services) };
            RegisterStoreComponent(builder, boundedContext);
            return builder;
        }

        private static void RegisterStoreComponent(ContainerBuilder builder, string boundedContext)
        {
            //HttpContext
            builder.RegisterType<HttpContextAccessor>()
                .As<IHttpContextAccessor>()
                .SingleInstance();

            //Identity
            builder.RegisterType<IdentityService>()
                .As<IIdentityService>();

            var keywords = new[] { "Gateway", "Factory", "Repository", "Mapper", "Service", "Handler" };

            if (assemblies.Any(o => o.Name.Equals(boundedContext)))
                builder.RegisterAssemblyTypes(assemblies.First(o => o.Name.Equals(boundedContext)).Assembly)
                    .Where(t => keywords.Any(o => t.Namespace.EndsWith(o) || t.Name.EndsWith(o)))
                    .AsImplementedInterfaces()
                    .UsingConstructor(new MostParametersConstructorSelector())
                    .InstancePerDependency();

            builder.Register(c =>
            {
                MQConfig config = c.Resolve<IOptions<MQConfig>>().Value;

                return Bus.Factory.CreateUsingRabbitMq(rabbit =>
                {
                    var host = rabbit.Host(new Uri(config.HostAddress), config.ConnectionName, settings =>
                    {
                        settings.Username(config.Username);
                        settings.Password(config.Password);
                    });
                    rabbit.ExchangeType = ExchangeType.Fanout;

                    if (assemblies.First(o => o.Name.Equals(boundedContext)).Receiver.Count > 0)
                        rabbit.ReceiveEndpoint(host, "Services", e => e.LoadFrom(c));
                });
            })
            .As<IBusControl>()
            .As<IBus>()
            .As<IPublishEndpoint>()
            .SingleInstance();
        }
    }
}