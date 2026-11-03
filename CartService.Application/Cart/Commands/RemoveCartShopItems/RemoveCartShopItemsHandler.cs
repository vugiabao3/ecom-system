using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.RemoveCartShopItems
{
    public class RemoveCartShopItemsHandler : IRequestHandler<RemoveCartShopItemsCommand, RemoveCartShopItemsResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public RemoveCartShopItemsHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<RemoveCartShopItemsResponse> Handle(RemoveCartShopItemsCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            foreach(var i in items.Where(i=>i.SellerId==r.SellerId)){ await _repo.DeleteAsync(i);}
            await _repo.SaveChangesAsync();
            return new RemoveCartShopItemsResponse{Message="Shop items removed"};
        }
    }
}
