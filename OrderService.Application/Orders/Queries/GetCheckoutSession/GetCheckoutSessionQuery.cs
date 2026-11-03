using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using OrderService.Application.DTOs;

namespace OrderService.Application.Orders.Queries.GetCheckoutSession
{
    public class GetCheckoutSessionQuery : IRequest<CheckoutSessionDto>
    {
        public Guid CheckoutId { get; set; }
    }
}
