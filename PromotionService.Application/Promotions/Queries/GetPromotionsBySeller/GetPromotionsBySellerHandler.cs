using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PromotionService.Application.Interfaces;
using PromotionService.Domain.Entities;

namespace PromotionService.Application.Promotions.Queries.GetPromotionsBySeller
{
    public class GetPromotionsBySellerHandler : IRequestHandler<GetPromotionsBySellerQuery, List<Promotion>>
    {
        private readonly IPromotionRepository _repo;

        public GetPromotionsBySellerHandler(IPromotionRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<Promotion>> Handle(GetPromotionsBySellerQuery request, CancellationToken cancellationToken)
        {
            var all = await _repo.GetAllAsync();
            return all.Where(p => p.SellerId == request.SellerId).ToList();
        }
    }
}
