using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.GetOrderById
{
    public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDto>
    {
        private readonly IOrderRepository _repo;
        private readonly ICurrentUserService _currentUser;

        public GetOrderByIdHandler(IOrderRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.Id);

            if (order == null)
                throw new Exception("Order not found");

            if (_currentUser.Role == "Seller" &&
                !order.Items.Any(i => i.SellerId == Guid.Parse(_currentUser.UserId)))
            {
                throw new Exception("Forbidden: not your order");
            }

            return new OrderDto
            {
                Id = order.Id,
                TotalPrice = order.TotalPrice,
                Status = order.Status.ToString().ToUpper(),
                PaymentStatus = order.PaymentStatus.ToString().ToUpper(),
                PaymentMethod = order.PaymentMethod.ToString(),
                ShippingFee = order.ShippingFee,
                SubTotal = order.SubTotal,
                Address = order.Address,
                Phone = order.Phone,
                ReceiverName = order.ReceiverName,
                Items = order.Items?.Select(x => new OrderItemDto
                {
                    ProductId = x.ProductId,
                    SellerId = x.SellerId,
                    Quantity = x.Quantity
                }).ToList()
            };
        }
    }
}
