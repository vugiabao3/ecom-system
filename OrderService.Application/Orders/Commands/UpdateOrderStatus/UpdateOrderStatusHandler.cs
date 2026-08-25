using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
    {
        private readonly IOrderRepository _repo;
        private readonly ICurrentUserService _currentUser;

        public UpdateOrderStatusHandler(IOrderRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _repo.GetByIdAsync(request.OrderId);

            if (order == null)
                throw new Exception("Order not found");

            if (!Enum.TryParse<OrderStatus>(request.Status, true, out var newStatus))
                throw new Exception("Invalid status");

            var validTransitions = new Dictionary<OrderStatus, OrderStatus[]>
            {
                [OrderStatus.Pending] = new[] { OrderStatus.Confirmed, OrderStatus.Cancelled },
                [OrderStatus.Confirmed] = new[] { OrderStatus.Preparing, OrderStatus.Cancelled },
                [OrderStatus.Preparing] = new[] { OrderStatus.ReadyForShipment },
                [OrderStatus.ReadyForShipment] = new[] { OrderStatus.Shipping },
                [OrderStatus.Shipping] = new[] { OrderStatus.Delivered, OrderStatus.DeliveryFailed },
                [OrderStatus.DeliveryFailed] = new[] { OrderStatus.Returned },
                [OrderStatus.Returned] = Array.Empty<OrderStatus>()
            };

            if (!validTransitions.ContainsKey(order.Status) ||
                !validTransitions[order.Status].Contains(newStatus))
            {
                throw new Exception($"Cannot transition from {order.Status} to {newStatus}");
            }

            if (_currentUser.Role == "Seller" &&
                !order.Items.Any(i => i.SellerId == Guid.Parse(_currentUser.UserId)))
            {
                throw new Exception("Forbidden: not your order");
            }

            order.Status = newStatus;
            order.UpdatedAt = DateTime.UtcNow;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = newStatus.ToString(),
                Note = request.Note
            });

            _repo.Update(order);
            await _repo.SaveChangesAsync();

            return true;
        }
    }
}
