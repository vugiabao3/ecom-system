using PaymentService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PaymentService.Application.Events
{
    public class PaymentFailedEvent
    {
        public Guid OrderId { get; set; }
        public string UserId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
