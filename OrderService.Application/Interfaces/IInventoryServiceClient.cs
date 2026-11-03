using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;

namespace OrderService.Application.Interfaces
{
    public interface IInventoryServiceClient
    {
        Task<InventoryDto> GetInventoryByProductId(Guid productId);
        Task<ReserveStockResponse> ReserveStock(Guid productId, int quantity);
    }

    public class InventoryDto
    {
        public Guid ProductId { get; set; }
        public int Available { get; set; }
        public int Reserved { get; set; }
    }

    public class ReserveStockResponse
    {
        public Guid ProductId { get; set; }
        public int Available { get; set; }
        public int Reserved { get; set; }
    }
}
