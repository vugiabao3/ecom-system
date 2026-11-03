using MediatR;
namespace CartService.Application.Cart.Commands.SelectCartItem
{
    public class SelectCartItemCommand : IRequest<SelectCartItemResponse> { public Guid ItemId { get; set; } }
    public class SelectCartItemResponse { public string Message { get; set; } = string.Empty; }
}