using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderService.Application.Interfaces;
using OrderService.Application.Events;
using OrderService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.EventHandlers
{
    public class PaymentFailedEventHandler
    {
        private readonly IOrderRepository _repo;

        public PaymentFailedEventHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(OrderService.Application.Events.PaymentFailedEvent @event)
        {
            var order = await _repo.GetByIdAsync(@event.OrderId);

            if (order == null) return;

            order.Status = OrderStatus.Cancelled;
            order.PaymentStatus = PaymentStatus.Failed;
            order.UpdatedAt = DateTime.UtcNow;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = OrderStatus.Cancelled.ToString(),
                Note = "Payment failed"
            });

            _repo.Update(order);
            await _repo.SaveChangesAsync();
        }
    }
}
