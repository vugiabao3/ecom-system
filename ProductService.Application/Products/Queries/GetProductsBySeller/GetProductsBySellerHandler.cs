using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Application.DTO;

namespace ProductService.Application.Products.Queries.GetProductsBySeller
{
    public class GetProductsBySellerHandler : IRequestHandler<GetProductsBySellerQuery, List<ProductDto>>
    {
        private readonly IProductRepository _repo;

        public GetProductsBySellerHandler(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ProductDto>> Handle(GetProductsBySellerQuery request, CancellationToken cancellationToken)
        {
            var allProducts = await _repo.GetAllAsync(1, int.MaxValue, null, null);
            var products = allProducts.Item1
                .Where(p => p.SellerId == request.SellerId && !p.IsDeleted)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    CategoryName = p.Category?.Name ?? "",
                    ImageUrl = p.ImageUrl,
                    SellerId = p.SellerId,
                    BrandId = p.BrandId,
                    BrandName = p.Brand?.Name ?? ""
                })
                .ToList();

            return products;
        }
    }
}
