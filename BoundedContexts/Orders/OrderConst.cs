using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Orders
{
    public class OrderConst : IConst
    {
        private readonly IServiceCollection services;

        public OrderConst(IServiceCollection services)
        {
            this.services = services;
        }

        public string Version => "0.0.1";

        public string Name => "Order";

        public Assembly Assembly => this.GetType().Assembly;

        public IDictionary<string, Type> Receiver => new Dictionary<string, Type>
        {
        };
    }
}