using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.GetOrdersBySellerId
{
    public record GetOrdersBySellerIdQuery(Guid SellerId) : IRequest<List<OrderDto>>;
}
