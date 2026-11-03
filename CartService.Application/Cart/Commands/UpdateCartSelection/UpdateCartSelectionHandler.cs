using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.UpdateCartSelection
{
    public class UpdateCartSelectionHandler : IRequestHandler<UpdateCartSelectionCommand, UpdateCartSelectionResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public UpdateCartSelectionHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<UpdateCartSelectionResponse> Handle(UpdateCartSelectionCommand r, CancellationToken ct)
        {
            foreach(var s in r.Selections)
            {
                var item = await _repo.GetItemByIdAsync(_user.UserId, s.ItemId);
                if(item==null) throw new Exception("Item not found");
                item.IsSelected = s.IsSelected;
                _repo.Update(item);
            }
            await _repo.SaveChangesAsync();
            return new UpdateCartSelectionResponse{Message="Selection updated"};
        }
    }
}
