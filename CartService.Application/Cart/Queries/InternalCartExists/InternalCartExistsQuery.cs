using MediatR;
namespace CartService.Application.Cart.Queries.InternalCartExists { public class InternalCartExistsQuery : IRequest<bool> { public string UserId { get; set; } = string.Empty; } }