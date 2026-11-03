using System;
using MediatR;
using ProductService.Application.Interfaces;

namespace ProductService.Application.Products.Commands.PatchProductStatus
{
    public class PatchProductStatusCommand : IRequest<PatchProductStatusResponse>
    {
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }

    public class PatchProductStatusResponse
    {
        public bool Success { get; set; }
    }

    public class PatchProductStatusHandler : IRequestHandler<PatchProductStatusCommand, PatchProductStatusResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ICacheService _cacheService;

        public PatchProductStatusHandler(IProductRepository productRepository, ICurrentUserService currentUser, ICacheService cacheService)
        {
            _productRepository = productRepository;
            _currentUser = currentUser;
            _cacheService = cacheService;
        }

        public async Task<PatchProductStatusResponse> Handle(PatchProductStatusCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != "Admin" && _currentUser.Role != "Seller")
                throw new Exception("Forbidden");

            var product = await _productRepository.GetByIdAsync(request.Id);
            if (product == null)
                throw new Exception("Product not found");

            if (product.CreatedBy != _currentUser.UserId && _currentUser.Role != "Admin")
                throw new Exception("Not owner");

            var allowed = new[] { "Draft", "Pending", "Active", "Inactive", "Suspended" };
            if (!allowed.Contains(request.Status))
                throw new Exception("Invalid status");

            product.Status = request.Status;
            product.IsActive = request.Status == "Active";
            await _productRepository.UpdateAsync(product);
            await _cacheService.RemoveAsync($"product_detail_{product.Id}");

            return new PatchProductStatusResponse { Success = true };
        }
    }
}