using MediatR;
namespace CartService.Application.Cart.Queries.GetCartShops
{
    public class GetCartShopsQuery : IRequest<List<Guid>> { }
}