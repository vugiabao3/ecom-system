using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.UnselectCartItem
{
    public class UnselectCartItemHandler : IRequestHandler<UnselectCartItemCommand, UnselectCartItemResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public UnselectCartItemHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<UnselectCartItemResponse> Handle(UnselectCartItemCommand r, CancellationToken ct)
        {
            var item = await _repo.GetItemByIdAsync(_user.UserId, r.ItemId);
            if(item==null) throw new Exception("Item not found");
            item.IsSelected=false; _repo.Update(item); await _repo.SaveChangesAsync();
            return new UnselectCartItemResponse{Message="Unselected"};
        }
    }
}
