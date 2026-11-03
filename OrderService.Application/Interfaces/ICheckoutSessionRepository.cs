using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Domain.Entities;

namespace OrderService.Application.Interfaces
{
    public interface ICheckoutSessionRepository
    {
        Task AddAsync(CheckoutSession session);
        Task<CheckoutSession> GetByIdAsync(Guid id);
        Task<CheckoutSession> GetByIdempotencyKeyAsync(string idempotencyKey);
        void Update(CheckoutSession session);
        Task SaveChangesAsync();
    }
}
