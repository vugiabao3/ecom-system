using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.SelectCartShop
{
    public class SelectCartShopHandler : IRequestHandler<SelectCartShopCommand, SelectCartShopResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public SelectCartShopHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<SelectCartShopResponse> Handle(SelectCartShopCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            foreach(var i in items.Where(i=>i.SellerId==r.SellerId)){ i.IsSelected=true; _repo.Update(i);}
            await _repo.SaveChangesAsync();
            return new SelectCartShopResponse{Message="Shop selected"};
        }
    }
}
