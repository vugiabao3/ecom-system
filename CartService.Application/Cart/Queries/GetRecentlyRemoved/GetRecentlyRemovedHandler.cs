using MediatR;
namespace CartService.Application.Cart.Queries.GetRecentlyRemoved { public class GetRecentlyRemovedHandler : IRequestHandler<GetRecentlyRemovedQuery, List<Guid>> { public Task<List<Guid>> Handle(GetRecentlyRemovedQuery r, CancellationToken ct) => Task.FromResult(new List<Guid>()); } }
