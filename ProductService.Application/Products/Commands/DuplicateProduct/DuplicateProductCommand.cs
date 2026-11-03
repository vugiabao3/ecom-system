using System;
using MediatR;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands.DuplicateProduct
{
    public class DuplicateProductCommand : IRequest<DuplicateProductResponse>
    {
        public Guid Id { get; set; }
    }

    public class DuplicateProductResponse
    {
        public Guid Id { get; set; }
        public bool Success { get; set; }
    }

    public class DuplicateProductHandler : IRequestHandler<DuplicateProductCommand, DuplicateProductResponse>
    {
        private readonly IProductRepository _repo;
        private readonly ICurrentUserService _currentUser;

        public DuplicateProductHandler(IProductRepository repo, ICurrentUserService currentUser)
        {
            _repo = repo;
            _currentUser = currentUser;
        }

        public async Task<DuplicateProductResponse> Handle(DuplicateProductCommand request, CancellationToken cancellationToken)
        {
            if (_currentUser.Role != "Admin" && _currentUser.Role != "Seller")
                throw new Exception("Forbidden");

            var original = await _repo.GetByIdAsync(request.Id);
            if (original == null)
                throw new Exception("Product not found");

            if (original.CreatedBy != _currentUser.UserId && _currentUser.Role != "Admin")
                throw new Exception("Not owner");

            var copy = new Product
            {
                Id = Guid.NewGuid(),
                Name = original.Name + " (Copy)",
                Price = original.Price,
                ImageUrl = original.ImageUrl,
                Description = original.Description,
                CategoryId = original.CategoryId,
                SellerId = original.SellerId,
                BrandId = original.BrandId,
                CreatedBy = _currentUser.UserId,
                CreatedAt = DateTime.UtcNow,
                IsActive = false,
                IsApproved = false,
                Status = "Draft"
            };

            await _repo.AddAsync(copy);

            return new DuplicateProductResponse { Id = copy.Id, Success = true };
        }
    }
}