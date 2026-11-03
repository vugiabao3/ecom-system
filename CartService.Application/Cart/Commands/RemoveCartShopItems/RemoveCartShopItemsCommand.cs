using MediatR;
namespace CartService.Application.Cart.Commands.RemoveCartShopItems
{
    public class RemoveCartShopItemsCommand : IRequest<RemoveCartShopItemsResponse> { public Guid SellerId { get; set; } }
    public class RemoveCartShopItemsResponse { public string Message { get; set; } = string.Empty; }
}