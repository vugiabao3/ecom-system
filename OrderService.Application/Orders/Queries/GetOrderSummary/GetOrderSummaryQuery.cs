using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderService.Application.Orders.Queries.GetOrderSummary
{
    public class GetOrderSummaryQuery : IRequest<DTOs.OrderSummaryDto>
    {
    }
}
