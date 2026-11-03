using MediatR;
namespace CartService.Application.Cart.Commands.PreviewCartDiscounts
{
    public class PreviewCartDiscountsHandler : IRequestHandler<PreviewCartDiscountsCommand, PreviewCartDiscountsResponse>
    {
        public async Task<PreviewCartDiscountsResponse> Handle(PreviewCartDiscountsCommand r, CancellationToken ct)
        {
            return new PreviewCartDiscountsResponse{Discount=0};
        }
    }
}
