using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.GetCartCount
{
    public class GetCartCountHandler : IRequestHandler<GetCartCountQuery, GetCartCountResponse>
    {
        private readonly ICartRepository _repo;
        private readonly ICurrentUserService _user;
        public GetCartCountHandler(ICartRepository repo, ICurrentUserService user) { _repo = repo; _user = user; }
        public async Task<GetCartCountResponse> Handle(GetCartCountQuery request, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            return new GetCartCountResponse { Count = items.Sum(i => i.Quantity) };
        }
    }
}