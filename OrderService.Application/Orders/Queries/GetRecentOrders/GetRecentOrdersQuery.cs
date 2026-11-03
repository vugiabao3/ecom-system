using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.GetRecentOrders
{
    public class GetRecentOrdersQuery : IRequest<DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Limit { get; set; } = 10;
    }
}
