using CartService.Application.Interfaces;
using MediatR;
using System.Linq;
namespace CartService.Application.Cart.Queries.GetCartShops
{
    public class GetCartShopsHandler : IRequestHandler<GetCartShopsQuery, List<Guid>>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetCartShopsHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<List<Guid>> Handle(GetCartShopsQuery r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            return items.Select(i=>i.SellerId).Distinct().ToList();
        }
    }
}
