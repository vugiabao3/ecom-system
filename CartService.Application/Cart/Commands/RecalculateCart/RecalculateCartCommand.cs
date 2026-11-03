using MediatR;
namespace CartService.Application.Cart.Commands.RecalculateCart
{
    public class RecalculateCartCommand : IRequest<RecalculateCartResponse> { }
    public class RecalculateCartResponse { public string Message { get; set; } = string.Empty; }
}