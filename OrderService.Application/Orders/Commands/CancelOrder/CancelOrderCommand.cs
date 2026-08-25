using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Domain.Entities;

namespace OrderService.Application.Orders.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public string? Reason { get; set; }
    }
}
