using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace InventoryService.Application.Inventory.Commands.AddStock
{
    public class AddStockCommand : IRequest<AddStockResponse>
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}