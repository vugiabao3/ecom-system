using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.GetPendingOrders
{
    public class GetPendingOrdersQuery : IRequest<DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
