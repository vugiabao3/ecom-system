using MediatR;
namespace CartService.Application.Cart.Commands.RemoveCartDevice { public class RemoveCartDeviceHandler : IRequestHandler<RemoveCartDeviceCommand, RemoveCartDeviceResponse> { public Task<RemoveCartDeviceResponse> Handle(RemoveCartDeviceCommand r, CancellationToken ct) => Task.FromResult(new RemoveCartDeviceResponse{Message="Removed"}); } }
