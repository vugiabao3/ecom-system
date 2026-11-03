using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.GetOrderTimeline
{
    public class GetOrderTimelineQuery : IRequest<List<DTOs.OrderStatusHistoryDto>>
    {
        public Guid OrderId { get; set; }
    }
}
