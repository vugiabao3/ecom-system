using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PromotionService.Application.Interfaces;

namespace PromotionService.Application.Promotions.Queries.GetAllPromotions;

public class GetAllPromotionsHandler
    : IRequestHandler<GetAllPromotionsQuery, List<PromotionDto>>
{
    private readonly IPromotionRepository _promotionRepository;

    public GetAllPromotionsHandler(
        IPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<List<PromotionDto>> Handle(
        GetAllPromotionsQuery query,
        CancellationToken cancellationToken)
    {
        var promotions =
            await _promotionRepository.GetAllAsync();

        return promotions
            .Select(x => new PromotionDto
            {
                Id = x.Id,
                Code = x.Code,
                DiscountPercent = x.DiscountPercent,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Quantity = x.Quantity
            })
            .ToList();
    }
}