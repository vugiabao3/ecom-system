using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Interfaces
{
    public interface IPaymentRepository
    {
        Task AddAsync(Payment payment);
        Task<Payment> GetByPaymentIdAsync(Guid paymentId);
        Task<Payment> GetByOrderIdAsync(Guid orderId);
        void Update(Payment payment);
        Task SaveChangesAsync();
    }
}
