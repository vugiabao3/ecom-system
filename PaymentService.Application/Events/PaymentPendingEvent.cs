using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.DTOs;

namespace PaymentService.Application.Events
{
    public class PaymentPendingEvent
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
    }
}
