using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace MakeUps
{
    public class MakeUpConst : IConst
    {
        public MakeUpConst(IServiceCollection services)
        {
            services.AddMassTransit(c =>
            {
            });
        }

        public string Version => "0.0.1";

        public string Name => "MakeUp";

        public Assembly Assembly => this.GetType().Assembly;

        public IDictionary<string, Type> Receiver => new Dictionary<string, Type>
        {
        };
    }
}