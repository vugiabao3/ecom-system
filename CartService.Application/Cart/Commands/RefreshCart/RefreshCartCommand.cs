using MediatR;
namespace CartService.Application.Cart.Commands.RefreshCart
{
    public class RefreshCartCommand : IRequest<RefreshCartResponse> { }
    public class RefreshCartResponse { public string Message { get; set; } = string.Empty; }
}