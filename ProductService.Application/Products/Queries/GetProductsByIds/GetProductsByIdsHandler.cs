using MediatR;
using ProductService.Application.DTO;
using ProductService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Queries.GetProductsByIds
{
    public class GetProductsByIdsHandler
     : IRequestHandler<GetProductsByIdsQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repo;

        public GetProductsByIdsHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProductDto>> Handle(
        GetProductsByIdsQuery request,
        CancellationToken cancellationToken)
        {
            var products = await _repo.GetByIdsAsync(request.Ids);

            if (products == null || !products.Any())
                return new List<ProductDto>();

            return products
                .Where(p => p != null)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name ?? "",
                    Price = p.Price,
                    ImageUrl = p.ImageUrl ?? "",

                    // ✅ FIX NULL CATEGORY
                    CategoryName = p.Category != null
                        ? p.Category.Name
                        : ""
                })
                .ToList();
        }
    }
}
