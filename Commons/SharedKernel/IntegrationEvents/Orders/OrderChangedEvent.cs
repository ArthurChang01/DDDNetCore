using SharedKernel.BaseClasses;
using SharedKernel.Interfaces;
using System.Collections.Generic;

namespace SharedKernel.IntegrationEvents.Orders
{
    public class OrderChangedEvent : ValueObject, IEvent
    {
        public OrderChangedEvent(string id, int versionNo,
            IEnumerable<OrderDetailEventModel> deteils)
        {
            this.Id = id;
            this.VersionNo = versionNo;
            this.Deteils = deteils;
        }

        public string Id { get; }

        public int VersionNo { get; }

        public IEnumerable<OrderDetailEventModel> Deteils { get; }



        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.VersionNo;
            yield return Deteils;
        }
    }
}