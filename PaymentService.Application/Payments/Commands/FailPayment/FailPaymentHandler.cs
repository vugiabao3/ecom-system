using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.Interfaces;
using PaymentService.Application.Events;
using PaymentService.Application.DTOs;
using PaymentService.Domain.Entities;
using EcomSystem.Contracts.Enums;

namespace PaymentService.Application.Payments.Commands.FailPayment
{
    public class FailPaymentHandler : IRequestHandler<FailPaymentCommand, bool>
    {
        private readonly IPaymentRepository _repo;
        private readonly IEventBus _eventBus;
        private readonly IOrderServiceClient _orderClient;

        public FailPaymentHandler(IPaymentRepository repo, IEventBus eventBus, IOrderServiceClient orderClient)
        {
            _repo = repo;
            _eventBus = eventBus;
            _orderClient = orderClient;
        }

        public async Task<bool> Handle(FailPaymentCommand request, CancellationToken cancellationToken)
        {
            var payment = await _repo.GetByPaymentIdAsync(request.PaymentId);

            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Status != PaymentStatus.Pending)
                return true;

            payment.Status = PaymentStatus.Failed;
            payment.FailureReason = request.FailureReason;

            _repo.Update(payment);
            await _repo.SaveChangesAsync();

            var order = await _orderClient.GetOrderById(payment.OrderId);

            await _eventBus.PublishAsync(new PaymentFailedEvent
            {
                OrderId = payment.OrderId,
                UserId = order?.UserId,
                Items = order?.Items ?? new List<OrderItemDto>()
            });

            return true;
        }
    }
}
