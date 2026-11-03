using MediatR;
using CartService.Domain.Entities;
namespace CartService.Application.Cart.Queries.GetCartItems
{
    public class GetCartItemsQuery : IRequest<List<CartItem>> { }
}