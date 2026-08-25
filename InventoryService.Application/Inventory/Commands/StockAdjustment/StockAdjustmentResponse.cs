using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Inventory.Commands.StockAdjustment
{
    public class StockAdjustmentResponse
    {
        public Guid ProductId { get; set; }
        public int Available { get; set; }
        public int Reserved { get; set; }
        public int Sold { get; set; }
    }
}
