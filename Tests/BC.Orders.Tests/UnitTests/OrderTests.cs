using FluentAssertions;
using NUnit.Framework;
using Orders.Models;
using Orders.Models.DTOs;
using SharedKernel.IntegrationEvents.Orders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BC.Orders.Tests.UnitTests
{
    [TestFixture]
    public class OrderTests
    {
        [Test]
        public void WhenCreateIsInvoked_ThenGenerateOrderCreatedEvent()
        {
            //arrange
            string id = Guid.NewGuid().ToString();
            string serverName = "John";
            IEnumerable<global::Orders.Models.OrderDetail> details = new List<global::Orders.Models.OrderDetail> {
                new global::Orders.Models.OrderDetail("Mocka",1,10)
            };
            OrderCreatedEvent expected = new OrderCreatedEvent(1, id, details.ToDictionary(k => k.ItemName, v => v.Quntaty)); ;
            Order target = new Order();

            //act
            target.Create(id, serverName, details);
            OrderCreatedEvent actual = target.IterateEvent().First() as OrderCreatedEvent;

            //assert
            actual.Should().Equals(expected);
        }

        [Test]
        public void WhenChangeItemsIsInvoked_ThenGenerateOrderChagedEvent()
        {
            //arrange
            var newItems = new List<OrderDetailDTO> {
                new OrderDetailDTO{ ItemName="Cake", Quntity=1, Price=25 }
            };
            var changeItems = new List<OrderDetailDTO>
            {
            };
            var removeItems = new List<OrderDetailDTO> { };
            Order target = new Order();
            OrderChangedEvent expect = new OrderChangedEvent(target.Id,
                1, new[] { new OrderDetailEventModel { ItemName = "Cake", Quntaty = 1, Price = 25 } }
             );

            //act
            target.ChangeItems(changeItems, newItems, removeItems);
            var actual = target.IterateEvent().First() as OrderChangedEvent;

            //assert
            actual.Should().Equals(expect);
        }

        [Test]
        public void WhenCancelIsInvoked_ThenGenerateOrderCancelEvent() {
            //arrange
            string id = Guid.NewGuid().ToString();
            Order target = new Order(id, "John", new List<OrderDetail>(), DateTimeOffset.Now);
            OrderCanceledEvent expect = new OrderCanceledEvent(1,id);

            //act
            target.Cancel();
            OrderCanceledEvent actual = target.IterateEvent().First() as OrderCanceledEvent;

            //assert
            target.Status.Should().Be(OrderStatus.Cancel);
            actual.Should().Equals(expect);
        }

    }
}
