using MediatR;
namespace CartService.Application.Cart.Commands.ValidateCart
{
    public class ValidateCartCommand : IRequest<ValidateCartResponse> { }
    public class ValidateCartResponse { public bool IsValid { get; set; } public string Message { get; set; } = string.Empty; }
}