using OrderService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Events;
using OrderService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace OrderService.Application.Orders.EventHandlers
{
    public class PaymentSucceededEventHandler
    {
        private readonly IOrderRepository _repo;

        public PaymentSucceededEventHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(PaymentSucceededEvent @event)
        {
            var order = await _repo.GetByIdAsync(@event.OrderId);

            if (order == null) return;
            if (order.Status == OrderStatus.Confirmed) return;

            order.Status = OrderStatus.Confirmed;
            order.PaymentStatus = PaymentStatus.Paid;
            order.UpdatedAt = DateTime.UtcNow;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = OrderStatus.Confirmed.ToString(),
                Note = "Payment succeeded"
            });

            _repo.Update(order);
            await _repo.SaveChangesAsync();
        }
    }
}
