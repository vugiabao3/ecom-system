using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionService.Application.Promotions.Commands.ApplyPromotion
{
    public class ApplyPromotionResponse
    {
        public bool isValid { get; set; }
        public decimal discountAmount { get; set; }
        public decimal finalAmount { get; set; }
        public string message { get; set; }
    }
}
