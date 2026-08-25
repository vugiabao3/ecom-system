using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PromotionService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace PromotionService.Application.Promotions.Queries.GetAllPromotions;

public class GetAllPromotionsHandler
    : IRequestHandler<GetAllPromotionsQuery, List<PromotionDto>>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetAllPromotionsHandler(
        IPromotionRepository promotionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _promotionRepository = promotionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<PromotionDto>> Handle(
        GetAllPromotionsQuery query,
        CancellationToken cancellationToken)
    {
        var role = _httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.Role)?.Value;

        var userIdClaim = _httpContextAccessor.HttpContext?.User
            ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var promotions = await _promotionRepository.GetAllAsync();

        // 🔥 Admin sees all promotions, Seller sees only their own
        if (role != "Admin" && !string.IsNullOrEmpty(userIdClaim))
        {
            var sellerId = Guid.Parse(userIdClaim);
            promotions = promotions.Where(p => p.SellerId == sellerId).ToList();
        }

        return promotions
            .Select(x => new PromotionDto
            {
                Id = x.Id,
                Code = x.Code,
                DiscountPercent = x.DiscountPercent,
                IsActive = x.IsActive,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Quantity = x.Quantity,
                SellerId = x.SellerId,
                BrandId = x.BrandId
            })
            .ToList();
    }
}