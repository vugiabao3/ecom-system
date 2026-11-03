using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.GetSelectedCartItems
{
    public class GetSelectedCartItemsHandler : IRequestHandler<GetSelectedCartItemsQuery, List<CartService.Domain.Entities.CartItem>>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetSelectedCartItemsHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<List<CartService.Domain.Entities.CartItem>> Handle(GetSelectedCartItemsQuery r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            return items.Where(i=>i.IsSelected).ToList();
        }
    }
}
