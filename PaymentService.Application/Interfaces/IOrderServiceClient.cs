using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.DTOs;

namespace PaymentService.Application.Interfaces
{
    public interface IOrderServiceClient
    {
        Task<OrderDto?> GetOrderById(Guid orderId);
    }
}
