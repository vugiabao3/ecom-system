using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PaymentService.Application.Payments.Commands.FailPayment
{
    public class FailPaymentCommand : IRequest<bool>
    {
        public Guid PaymentId { get; set; }
        public string? FailureReason { get; set; }
    }
}
