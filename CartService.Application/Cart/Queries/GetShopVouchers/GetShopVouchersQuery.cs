using MediatR;
namespace CartService.Application.Cart.Queries.GetShopVouchers { public class GetShopVouchersQuery : IRequest<List<string>> { public Guid SellerId { get; set; } } }