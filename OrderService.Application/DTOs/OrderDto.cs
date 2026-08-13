using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public string Address { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
    }
}
