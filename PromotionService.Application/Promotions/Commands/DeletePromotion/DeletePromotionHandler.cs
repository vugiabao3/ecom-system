using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PromotionService.Application.Interfaces;

namespace PromotionService.Application.Promotions.Commands.DeletePromotion;

public class DeletePromotionHandler
    : IRequestHandler<DeletePromotionCommand, DeletePromotionResponse>
{
    private readonly IPromotionRepository _promotionRepository;

    public DeletePromotionHandler(
        IPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<DeletePromotionResponse> Handle(
        DeletePromotionCommand command,
        CancellationToken cancellationToken)
    {
        var promotion =
            await _promotionRepository.GetByIdAsync(command.Id);

        if (promotion == null)
        {
            throw new Exception("Promotion not found");
        }

        await _promotionRepository.DeleteAsync(promotion);

        return new DeletePromotionResponse
        {
            PromotionId = promotion.Id,
            Message = "Promotion deleted successfully"
        };
    }
}