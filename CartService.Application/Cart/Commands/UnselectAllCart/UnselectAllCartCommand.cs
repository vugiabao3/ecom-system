using MediatR;
namespace CartService.Application.Cart.Commands.UnselectAllCart
{
    public class UnselectAllCartCommand : IRequest<UnselectAllCartResponse> { }
    public class UnselectAllCartResponse { public string Message { get; set; } = string.Empty; }
}