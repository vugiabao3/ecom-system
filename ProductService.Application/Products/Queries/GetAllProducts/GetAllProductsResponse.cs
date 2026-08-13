using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.DTO;
namespace ProductService.Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsResponse
    {
        public List<ProductDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }

   
}
