using SharedKernel.BaseClasses;
using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace SharedKernel.IntegrationEvents.Orders
{
    public class OrderCanceledEvent : ValueObject,IEvent
    {
        public OrderCanceledEvent(int versionNo, string ordereId)
        {
            this.Id = Guid.NewGuid().ToString();
            this.VersionNo = versionNo;
            this.OrderId = ordereId;
        }

        public string Id { get; }

        public int VersionNo { get; }

        public string OrderId { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.VersionNo;
            yield return this.OrderId;
        }
    }
}