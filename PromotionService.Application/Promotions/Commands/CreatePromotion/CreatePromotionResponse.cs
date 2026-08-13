using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionService.Application.Promotions.Commands.CreatePromotion;

public class CreatePromotionResponse
{
    public Guid PromotionId { get; set; }

    public string Message { get; set; } = default!;
}
