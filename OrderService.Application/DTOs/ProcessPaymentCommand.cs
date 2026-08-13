using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public class ProcessPaymentCommand
    {
        public Guid OrderId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } // MOMO, VNPAY, COD

        public List<OrderItemDto> Items { get; set; }
    }
}
