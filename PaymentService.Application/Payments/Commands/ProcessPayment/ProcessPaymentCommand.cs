using MediatR;
using PaymentService.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentService.Application.Payments.Commands.ProcessPayment
{
    public class ProcessPaymentCommand : IRequest<ProcessPaymentResponse>
    {
        public Guid OrderId { get; set; }
        //public decimal Amount { get; set; }
        // public string UserId { get; set; }   // 🔥 BẮT BUỘC

        //public string Token { get; set; }
        public string PaymentMethod { get; set; } // MOMO, VNPAY, COD
        //public List<OrderItemDto> Items { get; set; } // 🔥 thêm

    }
}
