using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.GetCartItems
{
    public class GetCartItemsHandler : IRequestHandler<GetCartItemsQuery, List<CartService.Domain.Entities.CartItem>>
    {
        private readonly ICartRepository _repo;
        private readonly ICurrentUserService _user;
        public GetCartItemsHandler(ICartRepository repo, ICurrentUserService user) { _repo = repo; _user = user; }
        public async Task<List<CartService.Domain.Entities.CartItem>> Handle(GetCartItemsQuery request, CancellationToken ct)
        {
            return await _repo.GetByUserIdAsync(_user.UserId);
        }
    }
}