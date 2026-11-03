using MediatR;
namespace CartService.Application.Cart.Commands.UnselectCartItem
{
    public class UnselectCartItemCommand : IRequest<UnselectCartItemResponse> { public Guid ItemId { get; set; } }
    public class UnselectCartItemResponse { public string Message { get; set; } = string.Empty; }
}