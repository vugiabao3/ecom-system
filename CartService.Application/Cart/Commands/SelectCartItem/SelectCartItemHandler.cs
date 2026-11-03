using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.SelectCartItem
{
    public class SelectCartItemHandler : IRequestHandler<SelectCartItemCommand, SelectCartItemResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public SelectCartItemHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<SelectCartItemResponse> Handle(SelectCartItemCommand r, CancellationToken ct)
        {
            var item = await _repo.GetItemByIdAsync(_user.UserId, r.ItemId);
            if(item==null) throw new Exception("Item not found");
            item.IsSelected=true; _repo.Update(item); await _repo.SaveChangesAsync();
            return new SelectCartItemResponse{Message="Selected"};
        }
    }
}
