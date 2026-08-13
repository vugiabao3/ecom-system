using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace CartService.Application.Interfaces
{
    public interface IInventoryServiceClient
    {
        Task ReserveStockAsync(
            Guid productId,
            int quantity
        );

        Task ReleaseStockAsync(
            Guid productId,
            int quantity
        );
    }
}