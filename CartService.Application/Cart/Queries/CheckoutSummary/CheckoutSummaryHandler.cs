using MediatR;
namespace CartService.Application.Cart.Queries.CheckoutSummary { public class CheckoutSummaryHandler : IRequestHandler<CheckoutSummaryQuery, CheckoutSummaryResponse> { public Task<CheckoutSummaryResponse> Handle(CheckoutSummaryQuery r, CancellationToken ct) => Task.FromResult(new CheckoutSummaryResponse{Message="Summary"}); } }
