using MediatR;
namespace CartService.Application.Cart.Queries.GetCartDiscounts
{
    public class GetCartDiscountsHandler : IRequestHandler<GetCartDiscountsQuery, List<string>>
    {
        public async Task<List<string>> Handle(GetCartDiscountsQuery r, CancellationToken ct) => new List<string>();
    }
}
