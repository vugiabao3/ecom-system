using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace InventoryService.Application.Events
{
    public class StockRejectedEvent
    {
        public Guid OrderId { get; set; }
        public List<Guid> FailedProducts { get; set; }
    }
}
