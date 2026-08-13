using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Application.Products.Commands;
using ProductService.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Commands.UpdateProduct
{


    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, UpdateProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly ICacheService _cacheService;
        private readonly IEventBus _eventBus;

        public UpdateProductHandler(
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

        public async Task<UpdateProductResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
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

            // 🔥 4. UPDATE FIELDS
            product.Name = request.Name;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            // 🔥 5. SAVE DB
            await _productRepository.UpdateAsync(product);

            // 🔥 6. INVALIDATE CACHE
            await _cacheService.RemoveAsync($"product_detail_{product.Id}");

            // 🔥 7. PUBLISH EVENT
            await _eventBus.PublishAsync(new ProductUpdatedEvent
            {
                ProductId = product.Id,
                Name = product.Name
            });

            return new UpdateProductResponse
            {
                Success = true
            };
        }
    }
}
