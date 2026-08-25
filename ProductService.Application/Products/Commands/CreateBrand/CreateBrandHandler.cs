using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using ProductService.Domain.Events;
using ProductService.Application.Interfaces;

namespace ProductService.Application.Products.Commands.CreateBrand
{
    public class CreateBrandHandler : IRequestHandler<CreateBrandCommand, CreateBrandResponse>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICurrentUserService _currentUserService;

        public CreateBrandHandler(IBrandRepository brandRepository, ICurrentUserService currentUserService)
        {
            _brandRepository = brandRepository;
            _currentUserService = currentUserService;
        }

        public async Task<CreateBrandResponse> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            var role = _currentUserService.Role;
            if (role != "Admin" && role != "Seller")
                throw new Exception("Forbidden");

            var brand = new Brand
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                LogoUrl = request.LogoUrl,
                SellerId = Guid.Parse(_currentUserService.UserId),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _brandRepository.AddAsync(brand);

            return new CreateBrandResponse
            {
                BrandId = brand.Id
            };
        }
    }
}
