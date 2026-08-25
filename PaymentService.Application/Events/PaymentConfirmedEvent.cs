using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PaymentService.Application.Events
{
    public class PaymentConfirmedEvent
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
