using MediatR;
namespace CartService.Application.Cart.Commands.RemoveInvalidItems
{
    public class RemoveInvalidItemsCommand : IRequest<RemoveInvalidItemsResponse> { }
    public class RemoveInvalidItemsResponse { public string Message { get; set; } = string.Empty; }
}