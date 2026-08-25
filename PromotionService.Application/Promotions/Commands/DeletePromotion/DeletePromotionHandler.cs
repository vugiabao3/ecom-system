using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PromotionService.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace PromotionService.Application.Promotions.Commands.DeletePromotion;

public class DeletePromotionHandler
    : IRequestHandler<DeletePromotionCommand, DeletePromotionResponse>
{
    private readonly IPromotionRepository _promotionRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeletePromotionHandler(
        IPromotionRepository promotionRepository,
        IHttpContextAccessor httpContextAccessor)
    {
        _promotionRepository = promotionRepository;
        _httpContextAccessor = httpContextAccessor;
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

        var role = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

        if (role != "Admin" && (userIdClaim == null || promotion.SellerId != Guid.Parse(userIdClaim)))
        {
            throw new Exception("Forbidden");
        }

        await _promotionRepository.DeleteAsync(promotion);

        return new DeletePromotionResponse
        {
            PromotionId = promotion.Id,
            Message = "Promotion deleted successfully"
        };
    }
}