using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQuery : IRequest<DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? StatusFilter { get; set; }
    }
}
