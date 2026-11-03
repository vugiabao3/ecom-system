using MediatR;
namespace CartService.Application.Cart.Queries.GetCartHistory { public class GetCartHistoryHandler : IRequestHandler<GetCartHistoryQuery, List<string>> { public Task<List<string>> Handle(GetCartHistoryQuery r, CancellationToken ct) => Task.FromResult(new List<string>()); } }
