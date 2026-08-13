using MediatR;
using PromotionService.Application.Promotions.Queries.GetAllPromotions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionService.Application
.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommand : IRequest<UpdatePromotionResponse>
{
    public Guid Id { get; set; }

    public string Code { get; set; } = default!;

    public decimal DiscountPercent { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public int Quantity { get; set; }

    public bool IsActive { get; set; }
}
