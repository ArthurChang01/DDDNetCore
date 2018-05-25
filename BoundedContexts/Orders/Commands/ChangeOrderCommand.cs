using Orders.Models.DTOs;
using System.Collections.Generic;

namespace Orders.Commands
{
    public class ChangeOrderCommand
    {
        public string OrderId { get; set; }

        public IEnumerable<OrderDetailDTO> NewItems { get; set; }

        public IEnumerable<OrderDetailDTO> ChangeItems { get; set; }

        public IEnumerable<OrderDetailDTO> RemoveItems { get; set; }
    }
}