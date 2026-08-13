using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace PromotionService.Application.Promotions.Commands.ApplyPromotion
{
    public class ApplyPromotionCommand : IRequest<ApplyPromotionResponse>
    {
        public string CouponCode { get; set; } = default!;
        public decimal TotalAmount { get; set; }
    }

}
