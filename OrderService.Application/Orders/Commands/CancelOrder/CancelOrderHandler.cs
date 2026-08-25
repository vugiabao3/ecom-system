using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;
using OrderService.Application.Events;
using OrderService.Application.DTOs;
using OrderService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.Commands.CancelOrder
{
    public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, bool>
    {
        private readonly IOrderRepository _repo;
        private readonly IEventBus _eventBus;
        private readonly ICurrentUserService _currentUser;

        public CancelOrderHandler(IOrderRepository repo, IEventBus eventBus, ICurrentUserService currentUser)
        {
            _repo = repo;
            _eventBus = eventBus;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.OrderId);

            if (order == null)
                throw new Exception("Order not found");

            if (order.Status == OrderStatus.Cancelled || order.Status == OrderStatus.Delivered)
                return false;

            if (_currentUser.Role == "Seller" &&
                !order.Items.Any(i => i.SellerId == Guid.Parse(_currentUser.UserId)))
            {
                throw new Exception("Forbidden");
            }

            if (_currentUser.Role != "Admin" && order.UserId != _currentUser.UserId)
            {
                throw new Exception("Forbidden");
            }

            order.Status = OrderStatus.Cancelled;
            order.UpdatedAt = DateTime.UtcNow;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = OrderStatus.Cancelled.ToString(),
                Note = request.Reason ?? "Order cancelled"
            });

            _repo.Update(order);
            await _repo.SaveChangesAsync();

            await _eventBus.PublishAsync("OrderCancelled", new OrderCancelledEvent
            {
                OrderId = order.Id,
                Items = order.Items.Select(x => new OrderItemDto
                {
                    ProductId = x.ProductId,
                    SellerId = x.SellerId,
                    Quantity = x.Quantity
                }).ToList()
            });

            return true;
        }
    }
}
