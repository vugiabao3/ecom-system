using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PromotionService.Application.Interfaces;
using PromotionService.Domain.Entities;
using MediatR;
namespace PromotionService.Application.Promotions.Commands.CreatePromotion;

public class CreatePromotionHandler
 : IRequestHandler<CreatePromotionCommand, CreatePromotionResponse>
{
    private readonly IPromotionRepository _promotionRepository;

    public CreatePromotionHandler(
        IPromotionRepository promotionRepository)
    {
        _promotionRepository = promotionRepository;
    }

    public async Task<CreatePromotionResponse> Handle(
        CreatePromotionCommand command)
    {
        var promotion = new Promotion
        {
            Id = Guid.NewGuid(),

            Code = command.Code,

            DiscountPercent = command.DiscountPercent,

            IsActive = true,

            StartDate = command.StartDate,

            EndDate = command.EndDate,

            Quantity = command.Quantity
        };

        await _promotionRepository.AddAsync(promotion);

        return new CreatePromotionResponse
        {
            PromotionId = promotion.Id,
            Message = "Promotion created successfully"
        };
    }
}
