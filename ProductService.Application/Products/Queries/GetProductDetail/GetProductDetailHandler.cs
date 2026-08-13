using MediatR;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Queries.GetProductDetail
{
    public class GetProductDetailHandler : IRequestHandler<GetProductDetailQuery, GetProductDetailResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICacheService _cacheService;

        public GetProductDetailHandler(
            IProductRepository productRepository,
            ICacheService cacheService)
        {
            _productRepository = productRepository;
            _cacheService = cacheService;
        }

        public async Task<GetProductDetailResponse> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
        {
            string cacheKey = $"product_detail_{request.Id}";

            // 🔥 1. CHECK CACHE
            var cached = await _cacheService.GetAsync<GetProductDetailResponse>(cacheKey);
            if (cached != null)
                return cached;

            // 🔥 2. CACHE MISS → QUERY DB
            var product = await _productRepository.GetProductDetailAsync(request.Id);

            if (product == null)
                throw new Exception("Product not found");

            var response = new GetProductDetailResponse
            {
                Id = product.Id,
                Name = product.Name,
                CategoryName = product.Category.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,

                // 🔥 Rating lấy từ cache riêng
                Rating = await _cacheService.GetAsync<double?>($"rating_{product.Id}") ?? 0
            };

            // 🔥 3. SET CACHE
            await _cacheService.SetAsync(cacheKey, response);

            return response;
        }
    }
}
