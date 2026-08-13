using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace InventoryService.Application.Inventory.Command.ReserveStock
{
    public class ReserveStockCommand : IRequest<ReserveStockResponse>
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}