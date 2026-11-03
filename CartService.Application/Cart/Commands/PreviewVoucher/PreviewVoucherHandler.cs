using MediatR;
namespace CartService.Application.Cart.Commands.PreviewVoucher { public class PreviewVoucherHandler : IRequestHandler<PreviewVoucherCommand, PreviewVoucherResponse> { public Task<PreviewVoucherResponse> Handle(PreviewVoucherCommand r, CancellationToken ct) => Task.FromResult(new PreviewVoucherResponse{Discount=0}); } }
