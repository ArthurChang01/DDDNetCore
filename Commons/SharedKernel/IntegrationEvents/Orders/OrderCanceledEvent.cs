using SharedKernel.Interfaces;
using System;

namespace SharedKernel.IntegrationEvents.Orders
{
    public class OrderCanceledEvent : IEvent
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
    }
}