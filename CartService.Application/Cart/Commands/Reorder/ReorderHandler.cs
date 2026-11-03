using MediatR;
namespace CartService.Application.Cart.Commands.Reorder { public class ReorderHandler : IRequestHandler<ReorderCommand, ReorderResponse> { public Task<ReorderResponse> Handle(ReorderCommand r, CancellationToken ct) => Task.FromResult(new ReorderResponse{Message="Reordered"}); } }
