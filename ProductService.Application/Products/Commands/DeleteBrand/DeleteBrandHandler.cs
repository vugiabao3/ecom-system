using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Commands.DeleteBrand
{
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand, bool>
    {
        private readonly IBrandRepository _brandRepository;
        private readonly ICurrentUserService _currentUserService;

        public DeleteBrandHandler(IBrandRepository brandRepository, ICurrentUserService currentUserService)
        {
            _brandRepository = brandRepository;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var role = _currentUserService.Role;
            if (role != "Admin" && role != "Seller")
                throw new Exception("Forbidden");

            var brand = await _brandRepository.GetByIdAsync(request.Id);
            if (brand == null)
                return false;

            if (brand.SellerId != Guid.Parse(_currentUserService.UserId) && role != "Admin")
                throw new Exception("Not owner");

            await _brandRepository.DeleteAsync(request.Id);
            return true;
        }
    }
}
