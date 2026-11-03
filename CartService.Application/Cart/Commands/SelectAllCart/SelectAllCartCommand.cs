using MediatR;
namespace CartService.Application.Cart.Commands.SelectAllCart
{
    public class SelectAllCartCommand : IRequest<SelectAllCartResponse> { }
    public class SelectAllCartResponse { public string Message { get; set; } = string.Empty; }
}