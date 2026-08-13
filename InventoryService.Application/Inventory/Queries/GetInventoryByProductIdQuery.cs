using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using InventoryService.Domain.Entities;

namespace InventoryService.Application.Inventory.Queries
{
    public class GetInventoryByProductIdQuery : IRequest<InventoryItem>
    {
        public Guid ProductId { get; set; }

        public GetInventoryByProductIdQuery(Guid productId)
        {
            ProductId = productId;
        }
    }
}