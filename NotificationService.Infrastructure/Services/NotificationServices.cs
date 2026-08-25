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

        public Task SendShippingCreated(Guid orderId, Guid shipmentId)
        {
            Console.WriteLine($"🚚 Shipping created for Order: {orderId}, Shipment: {shipmentId}");
            return Task.CompletedTask;
        }

        public Task SendDeliveryFailed(Guid orderId, string reason)
        {
            Console.WriteLine($"⚠️ Delivery FAILED for Order: {orderId}, Reason: {reason}");
            return Task.CompletedTask;
        }

        public Task SendDeliverySuccess(Guid orderId)
        {
            Console.WriteLine($"✅ Delivery SUCCESS for Order: {orderId}");
            return Task.CompletedTask;
        }

        public Task SendReturnOrder(Guid orderId, string reason)
        {
            Console.WriteLine($"🔄 Order RETURNED: {orderId}, Reason: {reason}");
            return Task.CompletedTask;
        }
    }
}