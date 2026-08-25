using MediatR;
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
    public class ReturnOrderEventHandler
    {
        private readonly IOrderRepository _repo;

        public ReturnOrderEventHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(ReturnOrderEvent @event)
        {
            var order = await _repo.GetByIdAsync(@event.OrderId);
            if (order == null) return;

            order.Status = OrderStatus.Returned;
            order.UpdatedAt = DateTime.UtcNow;

            order.StatusHistory.Add(new OrderStatusHistory
            {
                OrderId = order.Id,
                Status = OrderStatus.Returned.ToString(),
                Note = @event.Reason ?? "Order returned"
            });

            _repo.Update(order);
            await _repo.SaveChangesAsync();
        }
    }
}
