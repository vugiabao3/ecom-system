using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.Events
{
    public class PaymentFailedEvent
    {
        public Guid OrderId { get; set; }
    }
}
