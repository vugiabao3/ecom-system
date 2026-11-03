using System;
using MediatR;
using ProductService.Application.Interfaces;

namespace ProductService.Application.Products.Commands.PatchProduct
{
    public class PatchProductCommand : IRequest<PatchProductResponse>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public Guid? BrandId { get; set; }
    }

    public class PatchProductResponse
    {
        public bool Success { get; set; }
    }

    public class PatchProductHandler : IRequestHandler<PatchProductCommand, PatchProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ICacheService _cacheService;

        public PatchProductHandler(IProductRepository productRepository, ICurrentUserService currentUser, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _currentUser = currentUser;
            _cacheService = cacheService;
        }

        public async Task<PatchProductResponse> Handle(PatchProductCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != "Admin" && _currentUser.Role != "Seller")
                throw new Exception("Forbidden");

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                throw new Exception("Product not found");

            if (product.CreatedBy != _currentUser.UserId && _currentUser.Role != "Admin")
                throw new Exception("Not owner");

            if (request.Name != null) product.Name = request.Name;
            if (request.Price.HasValue) product.Price = request.Price.Value;
            if (request.CategoryId.HasValue) product.CategoryId = request.CategoryId.Value;
            if (request.BrandId.HasValue) product.BrandId = request.BrandId.Value;

            await _productRepository.UpdateAsync(product);
            await _cacheService.RemoveAsync($"product_detail_{product.Id}");

            return new PatchProductResponse { Success = true };
        }
    }
}