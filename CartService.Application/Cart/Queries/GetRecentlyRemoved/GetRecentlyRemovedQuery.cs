using MediatR;
namespace CartService.Application.Cart.Queries.GetRecentlyRemoved { public class GetRecentlyRemovedQuery : IRequest<List<Guid>> { } }