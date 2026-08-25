using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.GetOrdersByUserId
{
    public class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserIdQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _repo;

        public GetOrdersByUserIdHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repo.GetByUserIdAsync(request.UserId);

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                TotalPrice = o.TotalPrice,
                Status = o.Status.ToString().ToUpper(),
                PaymentStatus = o.PaymentStatus.ToString().ToUpper(),
                PaymentMethod = o.PaymentMethod.ToString(),
                ShippingFee = o.ShippingFee,
                SubTotal = o.SubTotal,
                Address = o.Address,
                Phone = o.Phone,
                ReceiverName = o.ReceiverName,
                Items = o.Items?.Select(x => new OrderItemDto
                {
                    ProductId = x.ProductId,
                    SellerId = x.SellerId,
                    Quantity = x.Quantity
                }).ToList()
            }).OrderByDescending(o => o.Id).ToList();
        }
    }
}
