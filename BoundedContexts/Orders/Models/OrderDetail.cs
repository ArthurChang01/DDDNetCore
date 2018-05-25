using SharedKernel.BaseClasses;
using System.Collections.Generic;

namespace Orders.Models
{
    public class OrderDetail : ValueObject
    {
        public OrderDetail(string itemName, int qty, int price)
        {
            this.ItemName = itemName;
            this.Quntaty = qty;
            this.Price = price;
        }

        public string ItemName { get; private set; }
        public int Quntaty { get; private set; }
        public int Price { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return this.ItemName;
            yield return this.Quntaty;
            yield return this.Price;
        }
    }
}