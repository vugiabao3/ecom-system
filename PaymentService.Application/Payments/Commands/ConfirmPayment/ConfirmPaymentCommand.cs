using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PaymentService.Application.Payments.Commands.ConfirmPayment
{
    public class ConfirmPaymentCommand : IRequest<bool>
    {
        public Guid PaymentId { get; set; }
    }
}
