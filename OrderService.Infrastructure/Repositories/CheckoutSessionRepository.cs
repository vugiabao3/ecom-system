using Microsoft.EntityFrameworkCore;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;
using OrderService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderService.Infrastructure.Repositories
{
    public class CheckoutSessionRepository : ICheckoutSessionRepository
    {
        private readonly AppDbContext _db;

        public CheckoutSessionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(CheckoutSession session)
        {
            _db.CheckoutSessions.Add(session);
            await _db.SaveChangesAsync();
        }

        public async Task<CheckoutSession> GetByIdAsync(Guid id)
        {
            return await _db.CheckoutSessions
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<CheckoutSession> GetByIdempotencyKeyAsync(string idempotencyKey)
        {
            return await _db.CheckoutSessions
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.IdempotencyKey == idempotencyKey);
        }

        public void Update(CheckoutSession session)
        {
            _db.CheckoutSessions.Update(session);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
