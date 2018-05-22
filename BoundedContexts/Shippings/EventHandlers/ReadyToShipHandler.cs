using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedKernel.IntegrationEvents.Sales;
using SharedKernel.OptionObjects;
using Shippings.Models;
using System;
using System.Threading.Tasks;

namespace Shippings.EventHandlers
{
    public class ReadyToShipHandler : IConsumer<OrderShippedReadyEvent>
    {
        private readonly IMongoCollection<Shipping> _collection;

        public ReadyToShipHandler(IOptions<DbConfig> opts)
        {
            MongoClient client = new MongoClient(opts.Value.ConnectionString);
            IMongoDatabase db = client.GetDatabase(opts.Value.DataBase);
            this._collection = db.GetCollection<Shipping>("Shippings");
        }

        public async Task Consume(ConsumeContext<OrderShippedReadyEvent> context)
        {
            await this._collection.InsertOneAsync(new Shipping
            {
                Id = Guid.NewGuid().ToString(),
                OrderRootId = context.Message.OrderRootId,
                OrderId = context.Message.OrderId,
                Status = ShippingStatus.WorkingOn
            });
        }
    }
}