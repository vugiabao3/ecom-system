using MediatR;
namespace CartService.Application.Cart.Commands.CheckoutPrepare { public class CheckoutPrepareHandler : IRequestHandler<CheckoutPrepareCommand, CheckoutPrepareResponse> { public Task<CheckoutPrepareResponse> Handle(CheckoutPrepareCommand r, CancellationToken ct) => Task.FromResult(new CheckoutPrepareResponse{Message="Prepared"}); } }
