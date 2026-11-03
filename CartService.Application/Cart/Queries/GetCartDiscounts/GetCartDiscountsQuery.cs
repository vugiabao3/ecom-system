using MediatR;
namespace CartService.Application.Cart.Queries.GetCartDiscounts
{
    public class GetCartDiscountsQuery : IRequest<List<string>> { }
}