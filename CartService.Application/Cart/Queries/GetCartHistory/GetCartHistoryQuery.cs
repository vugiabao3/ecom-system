using MediatR;
namespace CartService.Application.Cart.Queries.GetCartHistory { public class GetCartHistoryQuery : IRequest<List<string>> { } }