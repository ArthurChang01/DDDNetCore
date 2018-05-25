using LightBDD.NUnit3;
using NSubstitute;
using Orders.Commands;
using Orders.Models;
using Orders.Persistents;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BC.Orders.Tests.IntegrationTests.CancelOrder
{
    public partial class CancelOrder : FeatureFixture
    {
        private IOrderRepository _repository;

        private Task Given_template_method()
        {
            this._repository = NSubstitute.Substitute.For<IOrderRepository>();
            this._repository.Get(Arg.Any<string>())
                .Returns(Task.FromResult<Order>(
                    new Order("", "", new List<OrderDetail>(), DateTimeOffset.Now)
                    ));

            return Task.CompletedTask;
        }

        private async Task When_template_method()
        {
            var resul = await this._client.PostAsJsonAsync<CancelOrderCommand>("/Order/Cancel",
                new CancelOrderCommand { OrderId = "" });
        }

        private Task Then_template_method()
        {
            return Task.CompletedTask;
        }
    }
}