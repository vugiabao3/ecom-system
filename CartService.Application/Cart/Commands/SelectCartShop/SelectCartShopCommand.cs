using MediatR;
namespace CartService.Application.Cart.Commands.SelectCartShop
{
    public class SelectCartShopCommand : IRequest<SelectCartShopResponse> { public Guid SellerId { get; set; } }
    public class SelectCartShopResponse { public string Message { get; set; } = string.Empty; }
}