using OrderService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.Events;
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
            if (order.Status == "CONFIRMED") return; // 🔥 chống duplicate


            order.Status = "CONFIRMED";

            _repo.Update(order);
            await _repo.SaveChangesAsync();
        }
    }
}
