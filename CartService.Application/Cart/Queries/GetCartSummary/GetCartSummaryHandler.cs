using CartService.Application.Interfaces;
using MediatR;
using System.Linq;
namespace CartService.Application.Cart.Queries.GetCartSummary
{
    public class GetCartSummaryHandler : IRequestHandler<GetCartSummaryQuery, GetCartSummaryResponse>
    {
        private readonly ICartRepository _repo;
        private readonly ICurrentUserService _user;
        public GetCartSummaryHandler(ICartRepository repo, ICurrentUserService user) { _repo = repo; _user = user; }
        public async Task<GetCartSummaryResponse> Handle(GetCartSummaryQuery request, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            return new GetCartSummaryResponse
            {
                LineCount = items.Count,
                TotalQuantity = items.Sum(i => i.Quantity),
                Subtotal = items.Sum(i => i.PriceSnapshot * i.Quantity)
            };
        }
    }
}