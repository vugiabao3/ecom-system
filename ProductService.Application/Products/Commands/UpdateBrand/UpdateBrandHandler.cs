using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands.UpdateBrand
{
    public class UpdateBrandHandler : IRequestHandler<UpdateBrandCommand, bool>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICurrentUserService _currentUserService;

        public UpdateBrandHandler(IBrandRepository brandRepository, ICurrentUserService currentUserService)
        {
            _brandRepository = brandRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var role = _currentUserService.Role;
            if (role != "Admin" && role != "Seller")
                throw new Exception("Forbidden");

            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
                return false;

            if (brand.SellerId != Guid.Parse(_currentUserService.UserId) && role != "Admin")
                throw new Exception("Not owner");

            brand.Name = request.Name;
            brand.Description = request.Description;
            brand.LogoUrl = request.LogoUrl;

            await _brandRepository.UpdateAsync(brand);
            return true;
        }
    }
}
