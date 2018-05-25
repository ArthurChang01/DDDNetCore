using AutoFixture;
using LightBDD.NUnit3;
using NSubstitute;
using Orders.Commands;
using Orders.Models;
using Orders.Models.DTOs;
using Orders.Persistents;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BC.Orders.Tests.IntegrationTests.ChangeOrder
{
    public partial class ChangeOrder : FeatureFixture
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
            var newItems = (new Fixture()).Create<IEnumerable<OrderDetailDTO>>();
            var changeItems = (new Fixture()).Create<IEnumerable<OrderDetailDTO>>();
            var removeItems = (new Fixture()).Create<IEnumerable<OrderDetailDTO>>();

            var resul = await this._client.PostAsJsonAsync<ChangeOrderCommand>("/Order/Change",
                new ChangeOrderCommand { OrderId = "", NewItems = newItems, ChangeItems = changeItems, RemoveItems = removeItems });
        }

        private Task Then_template_method()
        {
            return Task.CompletedTask;
        }
    }
}