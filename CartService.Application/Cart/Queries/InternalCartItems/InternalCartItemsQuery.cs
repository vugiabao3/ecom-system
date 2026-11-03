using MediatR;
using CartService.Domain.Entities;
namespace CartService.Application.Cart.Queries.InternalCartItems { public class InternalCartItemsQuery : IRequest<List<CartItem>> { public string UserId { get; set; } = string.Empty; } }