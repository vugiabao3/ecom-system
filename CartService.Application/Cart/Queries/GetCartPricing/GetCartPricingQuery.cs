using MediatR;
namespace CartService.Application.Cart.Queries.GetCartPricing
{
    public class GetCartPricingQuery : IRequest<CartPricingResponse> { }
    public class CartPricingResponse { public decimal Subtotal { get; set; } public decimal Discount { get; set; } public decimal ShippingFee { get; set; } public decimal Total { get; set; } }
}