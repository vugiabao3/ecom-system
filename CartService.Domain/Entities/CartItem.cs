using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Domain.Entities
{
    public class CartItem
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }

        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
        public string ProductName { get; set; }


        public decimal PriceSnapshot { get; set; }
        public bool IsValid { get; set; } = true; // 🔥 thêm

    }
}
