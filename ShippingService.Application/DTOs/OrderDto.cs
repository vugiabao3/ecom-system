using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.DTOs
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string ReceiverName { get; set; }
        public string Phone { get; set; }
    }

}
