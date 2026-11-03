using CartService.Application.Interfaces;
using MediatR;
using System.Linq;
namespace CartService.Application.Cart.Queries.GetSelectedCartSummary
{
    public class GetSelectedCartSummaryHandler : IRequestHandler<GetSelectedCartSummaryQuery, GetSelectedCartSummaryResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user;
        public GetSelectedCartSummaryHandler(ICartRepository repo, ICurrentUserService user){_repo=repo;_user=user;}
        public async Task<GetSelectedCartSummaryResponse> Handle(GetSelectedCartSummaryQuery r, CancellationToken ct)
        {
            var items = (await _repo.GetByUserIdAsync(_user.UserId)).Where(i=>i.IsSelected).ToList();
            return new GetSelectedCartSummaryResponse { Count = items.Sum(i=>i.Quantity), Subtotal = items.Sum(i=>i.PriceSnapshot*i.Quantity) };
        }
    }
}
