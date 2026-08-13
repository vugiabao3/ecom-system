using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace OrderService.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<CreateOrderResponse>
    {
        public string Address { get; set; }
        public string Phone { get; set; }
        public string ReceiverName { get; set; }
        public string? CouponCode { get; set; } // 🔥 THÊM


    }
}
