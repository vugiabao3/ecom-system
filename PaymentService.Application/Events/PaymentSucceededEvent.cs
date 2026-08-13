using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.DTOs;
namespace PaymentService.Application.Events
{
    public class PaymentSucceededEvent
    {
        public string UserId { get; set; }   // 🔥 BẮT BUỘC

        public Guid OrderId { get; set; }
        public Guid PaymentId { get; set; }
        public List<OrderItemDto> Items { get; set; }

    }
}
