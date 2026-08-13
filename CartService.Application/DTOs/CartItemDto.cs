using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.DTOs
{
    public class CartItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        // 🔥 enriched từ ProductService
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        // 🔥 THÊM DÒNG NÀY
        public string ImageUrl { get; set; }
    }
}
