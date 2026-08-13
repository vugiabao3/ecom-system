using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Address { get; set; }
        public string Status { get; set; } = "PENDING";
        public decimal SubTotal { get; set; }   // trước giảm
        public decimal Discount { get; set; }   // giảm bao nhiêu
        public decimal TotalPrice { get; set; } // sau giảm
        public string Phone { get; set; }
        public string ReceiverName { get; set; }

        public List<OrderItem> Items { get; set; }
    }
}
