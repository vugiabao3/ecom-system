using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
namespace ProductService.Application.Products.Commands
{
  

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEventBus _eventBus;

        public CreateProductHandler(
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IEventBus eventBus)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _eventBus = eventBus;
        }

        public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // 🔥 1. LẤY USER INFO TỪ JWT
            var userId = _currentUserService.UserId;
            var role = _currentUserService.Role;

            // 🔥 2. CHECK ROLE
            if (role != "Admin" && role != "Seller")
                throw new Exception("Forbidden");

            // 🔥 3. VALIDATE
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("Name is required");

            if (request.Price <= 0)
                throw new Exception("Price must be > 0");

            // 🔥 4. CREATE ENTITY
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            };

            // 🔥 5. SAVE DB
            await _productRepository.AddAsync(product);

            // 🔥 6. PUBLISH EVENT
            await _eventBus.PublishAsync("ProductCreated", new
            {
                product.Id,
                product.Name
            });

            return new CreateProductResponse
            {
                ProductId = product.Id
            };
        }
    }
}
