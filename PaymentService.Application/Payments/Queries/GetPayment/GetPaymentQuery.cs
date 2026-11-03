using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;

namespace PaymentService.Application.Payments.Queries.GetPayment
{
    public class GetPaymentQuery : IRequest<Payment>
    {
        public Guid PaymentId { get; set; }
    }

    public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, Payment>
    {
        private readonly IPaymentRepository _repo;
        public GetPaymentHandler(IPaymentRepository repo) { _repo = repo; }
        public async Task<Payment> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetByPaymentIdAsync(request.PaymentId);
        }
    }
}
