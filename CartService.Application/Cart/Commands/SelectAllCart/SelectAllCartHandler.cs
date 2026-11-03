using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.SelectAllCart
{
    public class SelectAllCartHandler : IRequestHandler<SelectAllCartCommand, SelectAllCartResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public SelectAllCartHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<SelectAllCartResponse> Handle(SelectAllCartCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            foreach(var i in items){ i.IsSelected=true; _repo.Update(i);}
            await _repo.SaveChangesAsync();
            return new SelectAllCartResponse{Message="All selected"};
        }
    }
}
