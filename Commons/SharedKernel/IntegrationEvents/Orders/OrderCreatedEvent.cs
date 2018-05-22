using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace SharedKernel.IntegrationEvents.Orders
{
    public class OrderCreatedEvent : IEvent
    {
        public OrderCreatedEvent(int versionNo, string orderId, IDictionary<string, uint> details)
        {
            this.Id = Guid.NewGuid().ToString();
            this.VersionNo = versionNo;
            this.OrderId = orderId;
            this.OrderDetails = details;
        }

        public string Id { get; private set; }

        public int VersionNo { get; private set; }

        public string OrderId { get; private set; }

        public IDictionary<string, uint> OrderDetails { get; private set; }
    }
}