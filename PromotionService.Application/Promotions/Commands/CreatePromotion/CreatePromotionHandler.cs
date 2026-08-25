using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PromotionService.Application.Interfaces;
using PromotionService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
namespace PromotionService.Application.Promotions.Commands.CreatePromotion;

public class CreatePromotionHandler
  : IRequestHandler<CreatePromotionCommand, CreatePromotionResponse>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreatePromotionHandler(
        IPromotionRepository promotionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _promotionRepository = promotionRepository;
        _httpContextAccessor = httpContextAccessor;
    }

        public async Task<CreatePromotionResponse> Handle(
            CreatePromotionCommand command,
            CancellationToken cancellationToken)
        {
            // 🔥 SellerId is taken from the current authenticated user
            var userIdClaim = _httpContextAccessor.HttpContext?.User
                ?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var sellerId = command.SellerId;
            if (!string.IsNullOrEmpty(userIdClaim))
            {
                sellerId = Guid.Parse(userIdClaim);
            }

            var promotion = new Promotion
            {
                Id = Guid.NewGuid(),
                Code = command.Code,
                DiscountPercent = command.DiscountPercent,
                IsActive = true,
                StartDate = command.StartDate,
                EndDate = command.EndDate,
                Quantity = command.Quantity,
                SellerId = sellerId,
                BrandId = command.BrandId
            };

            await _promotionRepository.AddAsync(promotion);

            return new CreatePromotionResponse
            {
                PromotionId = promotion.Id,
                Message = "Promotion created successfully"
            };
        }
}
