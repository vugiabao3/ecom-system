using MediatR;
namespace CartService.Application.Cart.Commands.RestoreCartItem { public class RestoreCartItemHandler : IRequestHandler<RestoreCartItemCommand, RestoreCartItemResponse> { public Task<RestoreCartItemResponse> Handle(RestoreCartItemCommand r, CancellationToken ct) => Task.FromResult(new RestoreCartItemResponse{Message="Restored"}); } }
