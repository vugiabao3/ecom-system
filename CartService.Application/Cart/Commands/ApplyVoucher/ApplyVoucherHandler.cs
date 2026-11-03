using MediatR;
namespace CartService.Application.Cart.Commands.ApplyVoucher { public class ApplyVoucherHandler : IRequestHandler<ApplyVoucherCommand, ApplyVoucherResponse> { public Task<ApplyVoucherResponse> Handle(ApplyVoucherCommand r, CancellationToken ct) => Task.FromResult(new ApplyVoucherResponse{Message="Applied"}); } }
