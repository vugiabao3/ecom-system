using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.GetMyOrderDetail
{
    public class GetMyOrderDetailQuery : IRequest<DTOs.OrderDetailDto>
    {
        public Guid OrderId { get; set; }
    }
}
