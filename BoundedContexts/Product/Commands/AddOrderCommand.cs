using Orders.Models.DTOs;
using System.Collections.Generic;

namespace Orders.Commands
{
    public class AddOrderCommand
    {
        public string ServerName { get; set; }

        public IEnumerable<OrderDetailDTO> Details { get; set; }
    }
}