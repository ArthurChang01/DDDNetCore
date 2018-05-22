using SharedKernel.Interfaces;
using System;
using System.Collections.Generic;

namespace SharedKernel.IntegrationEvents.Orders
{
    public class OrderChangedEvent : IEvent
    {
        public OrderChangedEvent(string id, int versionNo,
            IEnumerable<Tuple<string, uint, double>> newItems,
            IEnumerable<Tuple<string, uint, double>> changeItems,
            IEnumerable<Tuple<string, uint, double>> removeItems)
        {
            this.Id = id;
            this.VersionNo = versionNo;
            this.NewItems = newItems;
            this.ChangeItems = changeItems;
            this.RemoveItems = removeItems;
        }

        public string Id { get; }

        public int VersionNo { get; }

        public IEnumerable<Tuple<string, uint, double>> NewItems { get; }

        public IEnumerable<Tuple<string, uint, double>> ChangeItems { get; }

        public IEnumerable<Tuple<string, uint, double>> RemoveItems { get; }
    }
}