using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InventoryService.Application.Interfaces;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Inventory.Commands.StockAdjustment
{
    public class StockAdjustmentCommand : IRequest<StockAdjustmentResponse>
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public string Type { get; set; }
    }
}
