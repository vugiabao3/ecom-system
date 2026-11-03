using MediatR;
namespace CartService.Application.Cart.Queries.GetAvailableVouchers { public class GetAvailableVouchersHandler : IRequestHandler<GetAvailableVouchersQuery, List<string>> { public Task<List<string>> Handle(GetAvailableVouchersQuery r, CancellationToken ct) => Task.FromResult(new List<string>()); } }
