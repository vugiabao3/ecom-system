using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.RemoveInvalidItems
{
    public class RemoveInvalidItemsHandler : IRequestHandler<RemoveInvalidItemsCommand, RemoveInvalidItemsResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public RemoveInvalidItemsHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<RemoveInvalidItemsResponse> Handle(RemoveInvalidItemsCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            foreach(var i in items.Where(i=>!i.IsValid)){ await _repo.DeleteAsync(i);}
            await _repo.SaveChangesAsync();
            return new RemoveInvalidItemsResponse{Message="Invalid items removed"};
        }
    }
}
