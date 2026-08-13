using MediatR;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.DTO;
namespace ProductService.Application.Products.Queries.SearchProduct
{
    public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, SearchProductsResponse>
    {
        private readonly IProductRepository _productRepository;

        public SearchProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<SearchProductsResponse> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
        {
            // 🔥 1. QUERY DB / ELASTIC SEARCH
            var result = await _productRepository.SearchAsync(request);

            return new SearchProductsResponse
            {
                Items = result.Items,
                TotalCount = result.TotalCount
            };
        }
    }
}
