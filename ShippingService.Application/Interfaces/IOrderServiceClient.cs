using ShippingService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingService.Application.Interfaces
{
    public interface IOrderServiceClient
    {
        Task<OrderDto> GetOrder(Guid orderId);
    }
}
