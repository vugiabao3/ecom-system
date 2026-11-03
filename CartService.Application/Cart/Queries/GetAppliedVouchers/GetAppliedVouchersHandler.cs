using MediatR;
namespace CartService.Application.Cart.Queries.GetAppliedVouchers { public class GetAppliedVouchersHandler : IRequestHandler<GetAppliedVouchersQuery, List<Guid>> { public Task<List<Guid>> Handle(GetAppliedVouchersQuery r, CancellationToken ct) => Task.FromResult(new List<Guid>()); } }
