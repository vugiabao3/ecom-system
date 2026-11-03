using MediatR;
namespace CartService.Application.Cart.Queries.GetSelectedCartSummary
{
    public class GetSelectedCartSummaryQuery : IRequest<GetSelectedCartSummaryResponse> { }
    public class GetSelectedCartSummaryResponse { public int Count { get; set; } public decimal Subtotal { get; set; } }
}