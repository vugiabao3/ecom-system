using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationService.Application.DTOs;
namespace NotificationService.Domain.Events
{
    public class PaymentSucceededEvent
    {
        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}