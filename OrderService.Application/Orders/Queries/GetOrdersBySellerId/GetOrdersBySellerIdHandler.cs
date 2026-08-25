using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.GetOrdersBySellerId
{
    public class GetOrdersBySellerIdHandler : IRequestHandler<GetOrdersBySellerIdQuery, List<OrderDto>>
    {
        private readonly IOrderRepository _repo;

        public GetOrdersBySellerIdHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<OrderDto>> Handle(GetOrdersBySellerIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _repo.GetBySellerIdAsync(request.SellerId);

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
