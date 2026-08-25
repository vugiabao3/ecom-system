using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
        public Guid SellerId { get; set; }
        public Guid BrandId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public Category Category { get; set; }
        public Brand Brand { get; set; }
    }
}
