using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.GetOrderConfirmation
{
    public class GetOrderConfirmationQuery : IRequest<DTOs.OrderDetailDto>
    {
        public Guid OrderId { get; set; }
    }
}
