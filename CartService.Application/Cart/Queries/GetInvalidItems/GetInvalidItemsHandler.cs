using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.GetInvalidItems
{
    public class GetInvalidItemsHandler : IRequestHandler<GetInvalidItemsQuery, List<CartService.Domain.Entities.CartItem>>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetInvalidItemsHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<List<CartService.Domain.Entities.CartItem>> Handle(GetInvalidItemsQuery r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            return items.Where(i=>!i.IsValid).ToList();
        }
    }
}
