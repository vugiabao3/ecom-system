using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PromotionService.Application.Interfaces;

namespace PromotionService.Application.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionHandler
    : IRequestHandler<UpdatePromotionCommand, UpdatePromotionResponse>
{
    private readonly IPromotionRepository _promotionRepository;

    public UpdatePromotionHandler(
        IPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
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