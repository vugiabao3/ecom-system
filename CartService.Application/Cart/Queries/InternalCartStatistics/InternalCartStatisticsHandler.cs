using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.InternalCartStatistics { public class InternalCartStatisticsHandler : IRequestHandler<InternalCartStatisticsQuery, InternalCartStatisticsResponse> { private readonly ICartRepository _repo; public InternalCartStatisticsHandler(ICartRepository repo){_repo=repo;} public async Task<InternalCartStatisticsResponse> Handle(InternalCartStatisticsQuery r, CancellationToken ct){ var all = await _repo.GetByUserIdAsync(""); return new InternalCartStatisticsResponse{TotalCarts=0, TotalItems=0}; } } }
