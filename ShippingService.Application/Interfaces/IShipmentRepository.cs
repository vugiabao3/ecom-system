using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShippingService.Domain.Entities;

namespace ShippingService.Application.Interfaces
{
    public interface IShipmentRepository
    {
        Task AddAsync(Shipment shipment);
        Task<Shipment?> GetByIdAsync(Guid id);
        Task<List<Shipment>> GetByShipperIdAsync(Guid? shipperId);
        Task SaveChangesAsync();
        Task<Shipment?> GetByOrderIdAsync(Guid orderId);
        void Update(Shipment shipment);
    }
}