using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Application.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Guid SellerId { get; set; }
        public string? Sku { get; set; }
        public string? VariantName { get; set; }
        public string? ImageUrl { get; set; }
    }
}
