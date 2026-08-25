using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PromotionService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PromotionService.Application.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionHandler
    : IRequestHandler<UpdatePromotionCommand, UpdatePromotionResponse>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UpdatePromotionHandler(
        IPromotionRepository promotionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _promotionRepository = promotionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UpdatePromotionResponse> Handle(
        UpdatePromotionCommand command,
        CancellationToken cancellationToken)
    {
        var promotion =
            await _promotionRepository.GetByIdAsync(command.Id);

        if (promotion == null)
        {
            throw new Exception("Promotion not found");
        }

        var role = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (role != "Admin" && (userIdClaim == null || promotion.SellerId != Guid.Parse(userIdClaim)))
        {
            throw new Exception("Forbidden");
        }

        promotion.Code = command.Code;
        promotion.DiscountPercent = command.DiscountPercent;
        promotion.StartDate = command.StartDate;
        promotion.EndDate = command.EndDate;
        promotion.Quantity = command.Quantity;
        promotion.IsActive = command.IsActive;

        await _promotionRepository.UpdateAsync(promotion);

        return new UpdatePromotionResponse
        {
            PromotionId = promotion.Id,
            Message = "Promotion updated successfully"
        };
    }
}