using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SharedKernel.IntegrationEvents.Shippings;
using SharedKernel.OptionObjects;
using Shippings.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Shippings.Gateways
{
    [Controller]
    public class ShippingGateway
    {
        private readonly IMongoCollection<Shipping> _collection;
        private readonly IBus _bus;

        public ShippingGateway(IOptions<DbConfig> opts, IBus bus)
        {
            MongoClient client = new MongoClient(opts.Value.ConnectionString);
            IMongoDatabase db = client.GetDatabase(opts.Value.DataBase);
            _collection = db.GetCollection<Shipping>("Shippings");

            this._bus = bus;
        }

        [HttpGet("/Shipping/Get")]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult(
                (await this._collection.Find(_ => true).ToListAsync()).Select(o => new
                {
                    Id = o.Id,
                    OrderId = o.OrderId,
                    Status = o.Status
                })
            );
        }

        [HttpGet("/Shipping/Get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await this._collection.Find(o => o.Id.Equals(id)).FirstOrDefaultAsync();
            return new OkObjectResult(new
            {
                Id = result.Id,
                OrderId = result.OrderId,
                Status = result.Status
            });
        }

        [HttpPost("/Shipping/Shipped/{id}")]
        public async Task<IActionResult> Shipped(string id)
        {
            var filter = Builders<Shipping>.Filter.Where(o => o.Id.Equals(id));
            var updated = Builders<Shipping>.Update.Set(o => o.Status, ShippingStatus.Finished);

            var result = await _collection.FindOneAndUpdateAsync(filter, updated);
            if (result != null)
                await this._bus.Publish(new OrderShippedEvent
                {
                    RootId = result.OrderRootId,
                    OrderId = result.OrderId
                });

            return new OkObjectResult(result != null);
        }
    }
}