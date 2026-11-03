using MediatR;
using CartService.Domain.Entities;
namespace CartService.Application.Cart.Queries.GetCartShopItems
{
    public class GetCartShopItemsQuery : IRequest<List<CartItem>> { public Guid SellerId { get; set; } }
}