using MediatR;
using PromotionService.Application.Promotions.Queries.GetAllPromotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionService.Application.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommand : IRequest<CreatePromotionResponse>
{
    public string Code { get; set; } = default!;

    public decimal DiscountPercent { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Quantity { get; set; }

    public Guid SellerId { get; set; }

    public Guid? BrandId { get; set; }
}
