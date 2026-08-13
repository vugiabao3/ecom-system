using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }

        public string ProductName { get; set; }
        public decimal Price { get; set; } // 🔥 snapshot
        public int Quantity { get; set; }
        public Guid OrderId { get; set; }   // 🔥 FIX FK
    }
}
