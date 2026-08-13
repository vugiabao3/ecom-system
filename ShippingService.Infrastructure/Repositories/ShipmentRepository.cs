using ShippingService.Application.Interfaces;
using ShippingService.Domain.Entities;
using ShippingService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace ShippingService.Infrastructure.Repositories
{
    public class ShipmentRepository : IShipmentRepository
    {
        private readonly AppDbContext _context;

        public ShipmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Shipment shipment)
        {
            await _context.Shipments.AddAsync(shipment);
        }
        public async Task<Shipment?> GetByIdAsync(Guid id)
        {
            return await _context.Shipments.FindAsync(id);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<Shipment?> GetByOrderIdAsync(Guid orderId)
        {
            return await _context.Shipments
                .FirstOrDefaultAsync(x => x.OrderId == orderId);
        }
    }
}