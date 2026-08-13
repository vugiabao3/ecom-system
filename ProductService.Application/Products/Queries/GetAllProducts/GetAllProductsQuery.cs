using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace ProductService.Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<GetAllProductsResponse>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public int? CategoryId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public string SortBy { get; set; } = "newest";
    }
}
