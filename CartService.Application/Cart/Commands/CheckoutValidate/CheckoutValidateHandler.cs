using MediatR;
namespace CartService.Application.Cart.Commands.CheckoutValidate { public class CheckoutValidateHandler : IRequestHandler<CheckoutValidateCommand, CheckoutValidateResponse> { public Task<CheckoutValidateResponse> Handle(CheckoutValidateCommand r, CancellationToken ct) => Task.FromResult(new CheckoutValidateResponse{IsValid=true,Message="Valid"}); } }
