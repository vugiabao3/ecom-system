using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Queries.GetProductDetail
{
    public class GetProductDetailResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }

        public double Rating { get; set; } // từ cache
        public string ImageUrl { get; set; }
    }
}
