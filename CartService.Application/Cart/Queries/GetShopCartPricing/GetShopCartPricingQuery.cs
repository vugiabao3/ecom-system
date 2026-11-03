using MediatR;
namespace CartService.Application.Cart.Queries.GetShopCartPricing
{
    public class GetShopCartPricingQuery : IRequest<CartPricingResponse> { public Guid SellerId { get; set; } }
    public class CartPricingResponse { public decimal Subtotal { get; set; } public decimal Discount { get; set; } public decimal ShippingFee { get; set; } public decimal Total { get; set; } }
}