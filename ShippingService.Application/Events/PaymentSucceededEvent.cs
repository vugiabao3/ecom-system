using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.Events
{
    public class PaymentSucceededEvent
    {
        public Guid OrderId { get; set; }
    }
}
