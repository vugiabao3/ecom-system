using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.GetCartItem
{
    public class GetCartItemHandler : IRequestHandler<GetCartItemQuery, CartService.Domain.Entities.CartItem>
    {
        private readonly ICartRepository _repo;
        private readonly ICurrentUserService _user;
        public GetCartItemHandler(ICartRepository repo, ICurrentUserService user) { _repo = repo; _user = user; }
        public async Task<CartService.Domain.Entities.CartItem> Handle(GetCartItemQuery request, CancellationToken ct)
        {
            var item = await _repo.GetItemByIdAsync(_user.UserId, request.ItemId);
            if (item == null) throw new Exception("Item not found");
            return item;
        }
    }
}