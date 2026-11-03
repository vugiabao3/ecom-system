using MediatR;
namespace CartService.Application.Cart.Commands.UnselectCartShop
{
    public class UnselectCartShopCommand : IRequest<UnselectCartShopResponse> { public Guid SellerId { get; set; } }
    public class UnselectCartShopResponse { public string Message { get; set; } = string.Empty; }
}