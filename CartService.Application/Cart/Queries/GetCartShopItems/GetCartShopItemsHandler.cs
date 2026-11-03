using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.GetCartShopItems
{
    public class GetCartShopItemsHandler : IRequestHandler<GetCartShopItemsQuery, List<CartService.Domain.Entities.CartItem>>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetCartShopItemsHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<List<CartService.Domain.Entities.CartItem>> Handle(GetCartShopItemsQuery r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            return items.Where(i=>i.SellerId==r.SellerId).ToList();
        }
    }
}
