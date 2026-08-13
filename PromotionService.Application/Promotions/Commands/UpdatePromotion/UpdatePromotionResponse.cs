using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionService.Application
.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionResponse
{
    public Guid PromotionId { get; set; }

    public string Message { get; set; }
        = default!;
}
