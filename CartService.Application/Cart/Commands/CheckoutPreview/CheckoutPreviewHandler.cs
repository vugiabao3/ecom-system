using MediatR;
namespace CartService.Application.Cart.Commands.CheckoutPreview { public class CheckoutPreviewHandler : IRequestHandler<CheckoutPreviewCommand, CheckoutPreviewResponse> { public Task<CheckoutPreviewResponse> Handle(CheckoutPreviewCommand r, CancellationToken ct) => Task.FromResult(new CheckoutPreviewResponse{Message="Preview"}); } }
