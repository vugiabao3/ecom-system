using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Application.Orders.Queries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _repo;

        public GetOrderByIdHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.Id);

            if (order == null)
                throw new Exception("Order not found");
            Console.WriteLine($"🔥 STATUS FROM ORDER: {order.Status}");
            return new OrderDto
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status,

                Address = order.Address,
                Phone = order.Phone,
                ReceiverName = order.ReceiverName
            };

        }
    }
}
