using MediatR;
namespace CartService.Application.Cart.Queries.GetAvailableVouchers { public class GetAvailableVouchersQuery : IRequest<List<string>> { } }