using Microsoft.EntityFrameworkCore;
using ShippingService.Application.Interfaces;
using ShippingService.Domain.Entities;
using ShippingService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShippingService.Infrastructure.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly AppDbContext _db;

        public ShipmentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Shipment shipment)
        {
            _db.Shipments.Add(shipment);
            await _db.SaveChangesAsync();
        }

        public async Task<Shipment?> GetByIdAsync(Guid id)
        {
            return await _db.Shipments.FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Shipment>> GetByShipperIdAsync(Guid? shipperId)
        {
            return await _db.Shipments
                .Where(s => s.ShipperId == shipperId)
                .ToListAsync();
        }

        public async Task<Shipment?> GetByOrderIdAsync(Guid orderId)
        {
            return await _db.Shipments.FirstOrDefaultAsync(s => s.OrderId == orderId);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Update(Shipment shipment)
        {
            _db.Shipments.Update(shipment);
        }
    }
}
