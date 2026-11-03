using CartService.Application.Interfaces;
using MediatR;
using System.Linq;
namespace CartService.Application.Cart.Queries.GetSelectedCartPricing
{
    public class GetSelectedCartPricingHandler : IRequestHandler<GetSelectedCartPricingQuery, CartPricingResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetSelectedCartPricingHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<CartPricingResponse> Handle(GetSelectedCartPricingQuery r, CancellationToken ct)
        {
            var items = (await _repo.GetByUserIdAsync(_user.UserId)).Where(i=>i.IsSelected).ToList();
            var sub = items.Sum(i=>i.PriceSnapshot*i.Quantity);
            return new CartPricingResponse{Subtotal=sub, Discount=0, ShippingFee=0, Total=sub};
        }
    }
}
