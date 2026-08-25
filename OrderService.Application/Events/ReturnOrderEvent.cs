using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Events
{
    public class ReturnOrderEvent
    {
        public Guid OrderId { get; set; }
        public string? Reason { get; set; }
    }
}
