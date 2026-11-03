using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.InternalCartItems { public class InternalCartItemsHandler : IRequestHandler<InternalCartItemsQuery, List<CartService.Domain.Entities.CartItem>> { private readonly ICartRepository _repo; public InternalCartItemsHandler(ICartRepository repo){_repo=repo;} public async Task<List<CartService.Domain.Entities.CartItem>> Handle(InternalCartItemsQuery r, CancellationToken ct) => await _repo.GetByUserIdAsync(r.UserId); } }
