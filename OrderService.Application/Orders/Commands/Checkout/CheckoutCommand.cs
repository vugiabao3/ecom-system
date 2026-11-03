using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace OrderService.Application.Orders.Commands.Checkout
{
    public class CheckoutCommand : IRequest<DTOs.CheckoutResponse>
    {
        public Guid? CheckoutId { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
        public string? CouponCode { get; set; }
        public string PaymentMethod { get; set; } = "QR";
        public string? IdempotencyKey { get; set; }
    }
}
