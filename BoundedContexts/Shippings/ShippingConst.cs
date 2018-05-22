using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.IntegrationEvents.Sales;
using SharedKernel.Interfaces;
using Shippings.EventHandlers;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Shippings
{
    public class ShippingConst : IConst
    {
        public ShippingConst(IServiceCollection services)
        {
            services.AddMassTransit(c =>
            {
                c.AddConsumer<ReadyToShipHandler>();
            });
        }

        public string Version => "0.0.1";

        public string Name => "Shipping";

        public Assembly Assembly => this.GetType().Assembly;

        public IDictionary<string, Type> Receiver => new Dictionary<string, Type>
        {
            { nameof(OrderShippedReadyEvent), typeof(ReadyToShipHandler)}
        };
    }
}