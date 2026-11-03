using MediatR;
using CartService.Domain.Entities;
namespace CartService.Application.Cart.Queries.GetInvalidItems
{
    public class GetInvalidItemsQuery : IRequest<List<CartItem>> { }
}