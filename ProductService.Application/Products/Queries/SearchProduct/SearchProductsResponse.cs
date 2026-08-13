using ProductService.Application.Products.Queries.GetAllProducts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.DTO;

namespace ProductService.Application.Products.Queries.SearchProduct
{
    public class SearchProductsResponse
    {
        public List<ProductDto> Items { get; set; }
        public int TotalCount { get; set; }
    }
}
