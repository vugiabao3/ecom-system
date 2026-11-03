using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Commands.ConfirmCheckout
{
    public class ConfirmCheckoutCommand : IRequest<DTOs.OrderConfirmationDto>
    {
        public Guid CheckoutId { get; set; }
    }
}
