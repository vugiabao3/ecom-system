using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; }
        public string? Note { get; set; }
    }
}
