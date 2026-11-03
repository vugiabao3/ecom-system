using MediatR;
using CartService.Domain.Entities;
namespace CartService.Application.Cart.Queries.GetSelectedCartItems
{
    public class GetSelectedCartItemsQuery : IRequest<List<CartItem>> { }
}