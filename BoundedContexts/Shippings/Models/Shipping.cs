namespace Shippings.Models
{
    public class Shipping
    {
        public string OrderId { get; set; }

        public ShippingStatus Status { get; set; }
        public string Id { get; set; }
        public string OrderRootId { get; set; }
    }
}