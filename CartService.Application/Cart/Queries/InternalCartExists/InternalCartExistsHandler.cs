using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.InternalCartExists { public class InternalCartExistsHandler : IRequestHandler<InternalCartExistsQuery, bool> { private readonly ICartRepository _repo; public InternalCartExistsHandler(ICartRepository repo){_repo=repo;} public async Task<bool> Handle(InternalCartExistsQuery r, CancellationToken ct){ var items = await _repo.GetByUserIdAsync(r.UserId); return items.Count>0; } } }
