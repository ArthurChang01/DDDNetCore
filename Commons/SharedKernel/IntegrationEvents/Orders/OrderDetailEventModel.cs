using System;

namespace SharedKernel.IntegrationEvents.Orders
{
    [Serializable]
    public class OrderDetailEventModel
    {
        public string ItemName { get; set; }
        public int Quntaty { get; set; }
        public int Price { get; set; }
    }
}
