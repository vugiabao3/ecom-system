using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace PromotionService.Application.Events
{
    public class PaymentSucceededEvent
    {
        public string UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid OrderId { get; set; }
    }
}