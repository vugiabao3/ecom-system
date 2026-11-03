using MediatR;
namespace CartService.Application.Cart.Queries.GetCartCount
{
    public class GetCartCountQuery : IRequest<GetCartCountResponse> { }
    public class GetCartCountResponse { public int Count { get; set; } }
}