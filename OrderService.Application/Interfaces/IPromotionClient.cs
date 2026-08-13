using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OrderService.Application.DTOs;
using System.Threading.Tasks;

namespace OrderService.Application.Interfaces
{
    public interface IPromotionClient
    {
        Task<ApplyPromotionResponse> Apply(string couponCode, decimal totalAmount);
    }
}