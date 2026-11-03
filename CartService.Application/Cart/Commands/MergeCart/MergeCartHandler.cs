using MediatR;
namespace CartService.Application.Cart.Commands.MergeCart { public class MergeCartHandler : IRequestHandler<MergeCartCommand, MergeCartResponse> { public Task<MergeCartResponse> Handle(MergeCartCommand r, CancellationToken ct) => Task.FromResult(new MergeCartResponse{Message="Merged"}); } }
