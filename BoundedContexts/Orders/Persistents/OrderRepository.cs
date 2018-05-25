using MassTransit;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Orders.Models;
using SharedKernel.Interfaces;
using SharedKernel.OptionObjects;
using System.Threading.Tasks;

namespace Orders.Persistents
{
    public interface IOrderRepository : IRepository<Order, string> { }

    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _collection;
        private readonly IBus _bus;

        public OrderRepository(IOptions<DbConfig> opts, IBus mediator)
        {
            MongoClient client = new MongoClient(opts.Value.ConnectionString);
            IMongoDatabase db = client.GetDatabase(opts.Value.DataBase);
            this._collection = db.GetCollection<Order>("Orders");

            this._bus = mediator;
        }

        public async Task<Order> Get(string id)
        {
            return await this._collection.Find(o => o.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task SaveChanges(Order root)
        {
            await this._collection.FindOneAndReplaceAsync<Order, Order>(
                o => o.Id.Equals(root.Id),
                root,
                new FindOneAndReplaceOptions<Order, Order> { IsUpsert = true });

            foreach (IEvent @event in root.IterateEvent())
                await this._bus.Publish(@event);
        }
    }
}