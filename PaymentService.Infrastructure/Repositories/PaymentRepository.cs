using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using EcomSystem.Contracts.Enums;

namespace PaymentService.Infrastructure.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _db;

        public PaymentRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(Payment payment)
        {
            await _db.Payments.AddAsync(payment);
        }

        public async Task<Payment> GetByPaymentIdAsync(Guid paymentId)
        {
            return await _db.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<Payment> GetByOrderIdAsync(Guid orderId)
        {
            return await _db.Payments.FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public void Update(Payment payment)
        {
            _db.Payments.Update(payment);
        }
    }
}
