using MediatR;
namespace CartService.Application.Cart.Queries.GetSyncStatus { public class GetSyncStatusHandler : IRequestHandler<GetSyncStatusQuery, GetSyncStatusResponse> { public Task<GetSyncStatusResponse> Handle(GetSyncStatusQuery r, CancellationToken ct) => Task.FromResult(new GetSyncStatusResponse()); } }
