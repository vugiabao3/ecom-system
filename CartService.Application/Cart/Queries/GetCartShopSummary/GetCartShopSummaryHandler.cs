using CartService.Application.Interfaces;
using MediatR;
using System.Linq;
namespace CartService.Application.Cart.Queries.GetCartShopSummary
{
    public class GetCartShopSummaryHandler : IRequestHandler<GetCartShopSummaryQuery, GetCartShopSummaryResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetCartShopSummaryHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<GetCartShopSummaryResponse> Handle(GetCartShopSummaryQuery r, CancellationToken ct)
        {
            var items = (await _repo.GetByUserIdAsync(_user.UserId)).Where(i=>i.SellerId==r.SellerId).ToList();
            return new GetCartShopSummaryResponse { Count = items.Sum(i=>i.Quantity), Subtotal = items.Sum(i=>i.PriceSnapshot*i.Quantity) };
        }
    }
}
