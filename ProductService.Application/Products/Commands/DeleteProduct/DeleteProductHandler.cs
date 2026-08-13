using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Commands.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, DeleteProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ICacheService _cacheService;
        private readonly IEventBus _eventBus;

        public DeleteProductHandler(
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

        public async Task<DeleteProductResponse> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1. CHECK ROLE
            if (_currentUser.Role != "Admin" && _currentUser.Role != "Seller")
                throw new Exception("Forbidden");

            // 🔥 2. GET PRODUCT
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
                throw new Exception("Product not found");

            // 🔥 3. CHECK OWNERSHIP
            if (product.CreatedBy != _currentUser.UserId)
                throw new Exception("Not owner");

            // 🔥 4. SOFT DELETE
            product.IsDeleted = true;

            // 🔥 5. SAVE DB
            await _productRepository.UpdateAsync(product);

            // 🔥 6. REMOVE CACHE
            await _cacheService.RemoveAsync($"product_detail_{product.Id}");

            // 🔥 7. PUBLISH EVENT
            await _eventBus.PublishAsync(new ProductDeletedEvent
            {
                Id = product.Id
            });

            return new DeleteProductResponse
            {
                Success = true
            };
        }
    }
}
