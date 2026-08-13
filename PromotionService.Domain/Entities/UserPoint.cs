using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromotionService.Domain.Entities
{
    public class UserPoint
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public int Points { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}