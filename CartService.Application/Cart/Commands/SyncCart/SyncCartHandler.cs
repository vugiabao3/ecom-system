using MediatR;
namespace CartService.Application.Cart.Commands.SyncCart { public class SyncCartHandler : IRequestHandler<SyncCartCommand, SyncCartResponse> { public Task<SyncCartResponse> Handle(SyncCartCommand r, CancellationToken ct) => Task.FromResult(new SyncCartResponse{Message="Synced"}); } }
