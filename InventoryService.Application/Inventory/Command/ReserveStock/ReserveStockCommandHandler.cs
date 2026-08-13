using InventoryService.Application.Interfaces;
using InventoryService.Application.Inventory.Command.ReserveStock;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryService.Application.Inventory.Command.ReserveStock
{
    public class ReserveStockHandler
        : IRequestHandler<
            ReserveStockCommand,
            ReserveStockResponse>
    {
        private readonly IInventoryRepository _repo;

        public ReserveStockHandler(
            IInventoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<ReserveStockResponse>
            Handle(
                ReserveStockCommand request,
                CancellationToken cancellationToken)
        {
            var item =
                await _repo.GetByProductIdAsync(
                    request.ProductId);

            // 🔥 không tồn tại inventory

            if (item == null)
            {
                throw new Exception(
                    "Inventory not found");
            }

            // 🔥 không đủ hàng

            if (item.Available <
                request.Quantity)
            {
                throw new Exception(
                    "Not enough stock");
            }

            // 🔥 reserve stock

            item.Available -=
                request.Quantity;

            item.Reserved +=
                request.Quantity;

            _repo.Update(item);

            await _repo.SaveChangesAsync();

            return new ReserveStockResponse
            {
                ProductId = item.ProductId,

                Available = item.Available,

                Reserved = item.Reserved
            };
        }
    }
}