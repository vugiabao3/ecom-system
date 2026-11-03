using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.SearchOrders
{
    public class SearchOrdersQuery : IRequest<DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        public string SearchTerm { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
