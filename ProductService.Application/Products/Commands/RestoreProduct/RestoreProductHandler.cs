using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Commands.RestoreProduct
{
    public class RestoreProductHandler : IRequestHandler<RestoreProductCommand, RestoreProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ICacheService _cacheService;
        private readonly IEventBus _eventBus;

        public RestoreProductHandler(
            IProductRepository productRepository,
            ICurrentUserService currentUser,
            ICacheService cacheService,
            IEventBus eventBus)
        {
            _productRepository = productRepository;
            _currentUser = currentUser;
            _cacheService = cacheService;
            _eventBus = eventBus;
        }

        public async Task<RestoreProductResponse> Handle(RestoreProductCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1. CHECK ROLE
            if (_currentUser.Role != "Admin" && _currentUser.Role != "Seller")
                throw new Exception("Forbidden");

            // 🔥 2. GET PRODUCT (include deleted)
            var product = await _productRepository.GetByIdIncludeDeletedAsync(request.Id);

            if (product == null)
                throw new Exception("Product not found");

            // 🔥 3. CHECK OWNERSHIP
            if (product.CreatedBy != _currentUser.UserId)
                throw new Exception("Not owner");

            // 🔥 4. RESTORE
            product.IsDeleted = false;

            // 🔥 5. SAVE DB
            await _productRepository.UpdateAsync(product);

            // 🔥 6. REFRESH CACHE (clear stale)
            await _cacheService.RemoveAsync($"product_detail_{product.Id}");

            // 🔥 7. PUBLISH EVENT

            await _eventBus.PublishAsync(new ProductRestoredEvent
            {
                ProductId = product.Id,
                Name = product.Name
            });
            return new RestoreProductResponse
            {
                Success = true
            };
        }
    }
}
