using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
namespace OrderService.Application.Interfaces
{
    public interface ICartServiceClient
    {
        Task<CartDto> GetCart();
        Task ClearCart(); // 🔥 thêm dòng này

    }
}