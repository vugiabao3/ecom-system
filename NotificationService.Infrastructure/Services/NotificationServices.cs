using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Infrastructure.Services
{

    public class NotificationServices
    {
        public Task SendOrderCreated(Guid orderId)
        {
            Console.WriteLine($"📢 Order {orderId} has been created!");
            return Task.CompletedTask;
        }
        public Task SendPaymentSuccess(Guid orderId)
        {
            Console.WriteLine($"💳 Payment SUCCESS for Order: {orderId}");
            return Task.CompletedTask;
        }

        public Task SendPaymentFailed(Guid orderId)
        {
            Console.WriteLine($"❌ PAYMENT FAILED: {orderId}");
            return Task.CompletedTask;
        }
    }
}