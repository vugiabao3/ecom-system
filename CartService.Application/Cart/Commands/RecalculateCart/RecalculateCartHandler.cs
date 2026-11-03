using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.RecalculateCart
{
    public class RecalculateCartHandler : IRequestHandler<RecalculateCartCommand, RecalculateCartResponse>
    {
        public async Task<RecalculateCartResponse> Handle(RecalculateCartCommand r, CancellationToken ct)
        {
            return new RecalculateCartResponse{Message="Recalculated"};
        }
    }
}
