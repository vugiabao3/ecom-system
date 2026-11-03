using MediatR;
namespace CartService.Application.Cart.Commands.RefreshCartItems
{
    public class RefreshCartItemsCommand : IRequest<RefreshCartItemsResponse> { public List<Guid> ItemIds { get; set; } = new(); }
    public class RefreshCartItemsResponse { public string Message { get; set; } = string.Empty; }
}