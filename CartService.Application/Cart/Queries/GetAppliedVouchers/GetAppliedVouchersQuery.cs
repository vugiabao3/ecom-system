using MediatR;
namespace CartService.Application.Cart.Queries.GetAppliedVouchers { public class GetAppliedVouchersQuery : IRequest<List<Guid>> { } }