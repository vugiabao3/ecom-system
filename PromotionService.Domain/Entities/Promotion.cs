using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionService.Domain.Entities
{
    public class Promotion
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = default!;
        public decimal DiscountPercent { get; set; }
        public bool IsActive { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Quantity { get; set; }
        public Guid SellerId { get; set; }
        public Guid? BrandId { get; set; }
    }
}
