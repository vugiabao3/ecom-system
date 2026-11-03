using CartService.Application.Interfaces;
using MediatR;
using System.Linq;
namespace CartService.Application.Cart.Queries.GetCartPricing
{
    public class GetCartPricingHandler : IRequestHandler<GetCartPricingQuery, CartPricingResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetCartPricingHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<CartPricingResponse> Handle(GetCartPricingQuery r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            var sub = items.Sum(i=>i.PriceSnapshot*i.Quantity);
            return new CartPricingResponse{Subtotal=sub, Discount=0, ShippingFee=0, Total=sub};
        }
    }
}
