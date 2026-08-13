using System;
using InventoryService.Application.DTOs;

namespace InventoryService.Application.Events
{
    public class OrderCreatedEvent
    {
        public Guid OrderId { get; set; }

        public List<OrderItemDto> Items { get; set; }
    }

    
}