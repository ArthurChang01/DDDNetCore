using MassTransit;
using SharedKernel.IntegrationEvents.Orders;
using System.Threading.Tasks;

namespace MakeUps.EventHandlers
{
    public class CancelOrderHandler : IConsumer<OrderCanceledEvent>
    {
        public CancelOrderHandler()
        {
        }

        public Task Consume(ConsumeContext<OrderCanceledEvent> context)
        {
            throw new System.NotImplementedException();
        }
    }
}