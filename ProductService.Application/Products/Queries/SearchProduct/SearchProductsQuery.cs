using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Queries.SearchProduct
{
    public class SearchProductsQuery : IRequest<SearchProductsResponse>
    {
        public string Keyword { get; set; }

        public int? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }

        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string SortBy { get; set; } = "relevance";
    }
}
