using MediatR;
namespace CartService.Application.Cart.Queries.GetCartShopSummary
{
    public class GetCartShopSummaryQuery : IRequest<GetCartShopSummaryResponse> { public Guid SellerId { get; set; } }
    public class GetCartShopSummaryResponse { public int Count { get; set; } public decimal Subtotal { get; set; } }
}