using SharedKernel.BaseClasses;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace SharedKernel.IntegrationEvents.Orders
{
    public class OrderCreatedEvent : ValueObject, IEvent
    {
        public OrderCreatedEvent(int versionNo, string orderId, IDictionary<string, int> details)
        {
            this.Id = Guid.NewGuid().ToString();
            this.VersionNo = versionNo;
            this.OrderId = orderId;
            this.OrderDetails = details;
        }

        public string Id { get; private set; }

        public int VersionNo { get; private set; }

        public string OrderId { get; private set; }

        public IDictionary<string, int> OrderDetails { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.VersionNo;
            yield return this.OrderId;
            yield return this.OrderDetails;
        }
    }
}