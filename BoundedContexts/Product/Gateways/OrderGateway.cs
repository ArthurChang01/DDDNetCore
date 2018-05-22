using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orders.Commands;
using Orders.Models;
using Orders.Persistents;
using System.Linq;
using System.Threading.Tasks;

namespace Orders.Gateways
{
    [Controller]
    public class OrderGateway
    {
        private readonly IOrderRepository _repository;

        public OrderGateway(IOrderRepository repository)
        {
            this._repository = repository;
        }

        [Authorize]
        [HttpGet("/Order/Get/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            return new OkObjectResult(
                this._repository.Get(id)
            );
        }

        [Authorize]
        [HttpPost("/Order/Create")]
        public async Task<IActionResult> Add([FromBody]AddOrderCommand cmd)
        {
            Order order = new Order();

            order.Create(
                cmd.ServerName,
                cmd.Details.Select(o => new OrderDetail(o.ItemName, o.Quntity, o.Price))
            );
            await this._repository.SaveChanges(order);

            return new OkObjectResult(order.Id);
        }

        [Authorize]
        [HttpPost("/Order/Change")]
        public async Task<IActionResult> Change([FromBody]ChangeOrderCommand cmd)
        {
            Order order = await this._repository.Get(cmd.OrderId);
            if (order == null)
                return new BadRequestObjectResult("order is not exit!");

            order.ChangeItems(cmd.ChangeItems, cmd.NewItems, cmd.RemoveItems);
            await this._repository.SaveChanges(order);

            return new OkResult();
        }

        [Authorize]
        [HttpDelete("/Order/Cancel")]
        public async Task<IActionResult> Cancel([FromBody]CancelOrderCommand cmd)
        {
            Order order = await this._repository.Get(cmd.OrderId);

            order.Cancel();
            await this._repository.SaveChanges(order);

            return new OkResult();
        }
    }
}