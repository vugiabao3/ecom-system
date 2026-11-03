using MediatR;
namespace CartService.Application.Cart.Queries.GetShopVouchers { public class GetShopVouchersHandler : IRequestHandler<GetShopVouchersQuery, List<string>> { public Task<List<string>> Handle(GetShopVouchersQuery r, CancellationToken ct) => Task.FromResult(new List<string>()); } }
