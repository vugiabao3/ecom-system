using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.InternalClearCart { public class InternalClearCartHandler : IRequestHandler<InternalClearCartCommand, InternalClearCartResponse> { private readonly ICartRepository _repo; public InternalClearCartHandler(ICartRepository repo){_repo=repo;} public async Task<InternalClearCartResponse> Handle(InternalClearCartCommand r, CancellationToken ct){ await _repo.DeleteByUserIdAsync(r.UserId); return new InternalClearCartResponse{Message="Cleared"}; } } }
