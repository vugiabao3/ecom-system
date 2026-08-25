using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.GetOrdersByUserId
{
    public record GetOrdersByUserIdQuery(string UserId) : IRequest<List<OrderDto>>;
}
