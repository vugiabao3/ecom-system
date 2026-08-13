using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderService.Application.Interfaces;
using OrderService.Application.Events;

namespace OrderService.Application.Orders.EventHandlers
{
    public class PaymentFailedEventHandler
    {
        private readonly IOrderRepository _repo;

        public PaymentFailedEventHandler(IOrderRepository repo)
        {
            _repo = repo;
        }

        public async Task Handle(PaymentFailedEvent @event)
        {
            // 🔥 tìm order
            var order = await _repo.GetByIdAsync(@event.OrderId);

            if (order == null) return;

            // 🔥 update trạng thái
            order.Status = "CANCELLED";    

            _repo.Update(order);
            await _repo.SaveChangesAsync();
        }
    }
}
