using MediatR;
namespace CartService.Application.Cart.Commands.RemoveVoucher { public class RemoveVoucherHandler : IRequestHandler<RemoveVoucherCommand, RemoveVoucherResponse> { public Task<RemoveVoucherResponse> Handle(RemoveVoucherCommand r, CancellationToken ct) => Task.FromResult(new RemoveVoucherResponse{Message="Removed"}); } }
