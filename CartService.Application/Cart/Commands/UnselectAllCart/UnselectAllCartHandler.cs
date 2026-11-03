using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.UnselectAllCart
{
    public class UnselectAllCartHandler : IRequestHandler<UnselectAllCartCommand, UnselectAllCartResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public UnselectAllCartHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<UnselectAllCartResponse> Handle(UnselectAllCartCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            foreach(var i in items){ i.IsSelected=false; _repo.Update(i);}
            await _repo.SaveChangesAsync();
            return new UnselectAllCartResponse{Message="All unselected"};
        }
    }
}
