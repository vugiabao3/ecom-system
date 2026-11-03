using MediatR;
namespace CartService.Application.Cart.Commands.ValidateSelectedCart
{
    public class ValidateSelectedCartCommand : IRequest<ValidateSelectedCartResponse> { }
    public class ValidateSelectedCartResponse { public bool IsValid { get; set; } public string Message { get; set; } = string.Empty; }
}