using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.FilterOrdersByStatus
{
    public class FilterOrdersByStatusQuery : IRequest<DTOs.PaginatedResponse<DTOs.OrderListItemDto>>
    {
        public string StatusFilter { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
