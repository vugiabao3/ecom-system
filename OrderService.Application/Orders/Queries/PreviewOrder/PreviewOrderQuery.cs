using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.PreviewOrder
{
    public class PreviewOrderQuery : IRequest<PreviewResponse>
    {
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? ReceiverName { get; set; }
        public string? CouponCode { get; set; }
        public string PaymentMethod { get; set; } = "QR";
    }
}
