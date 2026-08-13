using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Domain.Entities
{
    public class InventoryItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public int Available { get; set; } // còn bao nhiêu
        public int Reserved { get; set; }  // đã giữ bao nhiêu
    }
}