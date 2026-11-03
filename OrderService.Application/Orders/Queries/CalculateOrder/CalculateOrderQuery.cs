using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.CalculateOrder
{
    public class CalculateOrderQuery : IRequest<CalculateResponse>
    {
        public string? CouponCode { get; set; }
    }
}
