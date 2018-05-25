using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Orders;

namespace API
{
    public partial class Startup
    {
        public void ConfigureMediatR(IServiceCollection services)
        {
            var arAssembly = new[] { typeof(OrderConst) };

            services.AddMediatR(arAssembly);
        }
    }
}