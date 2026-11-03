using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.UnselectCartShop
{
    public class UnselectCartShopHandler : IRequestHandler<UnselectCartShopCommand, UnselectCartShopResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public UnselectCartShopHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<UnselectCartShopResponse> Handle(UnselectCartShopCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            foreach(var i in items.Where(i=>i.SellerId==r.SellerId)){ i.IsSelected=false; _repo.Update(i);}
            await _repo.SaveChangesAsync();
            return new UnselectCartShopResponse{Message="Shop unselected"};
        }
    }
}
