using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.Events
{
    public class PaymentPendingEvent
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}
