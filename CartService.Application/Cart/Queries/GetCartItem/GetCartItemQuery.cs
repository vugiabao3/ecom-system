using MediatR;
using CartService.Domain.Entities;
namespace CartService.Application.Cart.Queries.GetCartItem
{
    public class GetCartItemQuery : IRequest<CartItem>
    {
        public Guid ItemId { get; set; }
    }
}