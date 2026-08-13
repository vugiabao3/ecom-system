using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PaymentService.Application.Payments.Commands.ProcessPayment
{
    public class ProcessPaymentResponse
    {
        public Guid PaymentId { get; set; }
        public string Status { get; set; }
    }
}