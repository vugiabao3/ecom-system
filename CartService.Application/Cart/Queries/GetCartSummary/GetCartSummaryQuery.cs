using MediatR;
namespace CartService.Application.Cart.Queries.GetCartSummary
{
    public class GetCartSummaryQuery : IRequest<GetCartSummaryResponse> { }
    public class GetCartSummaryResponse
    {
        public int LineCount { get; set; }
        public int TotalQuantity { get; set; }
        public decimal Subtotal { get; set; }
    }
}