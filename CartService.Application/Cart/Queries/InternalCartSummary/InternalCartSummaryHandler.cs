using MediatR;
namespace CartService.Application.Cart.Queries.InternalCartSummary { public class InternalCartSummaryHandler : IRequestHandler<InternalCartSummaryQuery, InternalCartSummaryResponse> { public Task<InternalCartSummaryResponse> Handle(InternalCartSummaryQuery r, CancellationToken ct) => Task.FromResult(new InternalCartSummaryResponse{Message="Summary"}); } }
