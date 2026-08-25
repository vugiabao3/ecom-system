using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Queries.GetBrandsBySeller
{
    public class GetBrandsBySellerHandler : IRequestHandler<GetBrandsBySellerQuery, List<Brand>>
    {
        private readonly IBrandRepository _repo;

        public GetBrandsBySellerHandler(IBrandRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Brand>> Handle(GetBrandsBySellerQuery request, CancellationToken cancellationToken)
        {
            var allBrands = await _repo.GetAllAsync();
            return allBrands
                .Where(b => b.SellerId == request.SellerId)
                .ToList();
        }
    }
}
