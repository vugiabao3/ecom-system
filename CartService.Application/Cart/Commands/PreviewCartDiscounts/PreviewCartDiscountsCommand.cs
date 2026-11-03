using MediatR;
namespace CartService.Application.Cart.Commands.PreviewCartDiscounts
{
    public class PreviewCartDiscountsCommand : IRequest<PreviewCartDiscountsResponse> { public string CouponCode { get; set; } = string.Empty; }
    public class PreviewCartDiscountsResponse { public decimal Discount { get; set; } }
}