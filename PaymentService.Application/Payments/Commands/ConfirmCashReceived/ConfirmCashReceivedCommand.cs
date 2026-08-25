using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Payments.Commands.ConfirmCashReceived
{
    public class ConfirmCashReceivedCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
    }
}
