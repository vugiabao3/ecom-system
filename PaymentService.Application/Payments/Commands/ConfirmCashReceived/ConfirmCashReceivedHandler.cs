using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Application.Events;
using PaymentService.Application.DTOs;
using EcomSystem.Contracts.Enums;

namespace PaymentService.Application.Payments.Commands.ConfirmCashReceived
{
    public class ConfirmCashReceivedHandler : IRequestHandler<ConfirmCashReceivedCommand, bool>
    {
        private readonly IPaymentRepository _repo;
        private readonly IEventBus _eventBus;
        private readonly IOrderServiceClient _orderClient;

        public ConfirmCashReceivedHandler(IPaymentRepository repo, IEventBus eventBus, IOrderServiceClient orderClient)
        {
            _repo = repo;
            _eventBus = eventBus;
            _orderClient = orderClient;
        }

        public async Task<bool> Handle(ConfirmCashReceivedCommand request, CancellationToken cancellationToken)
        {
            var payment = await _repo.GetByOrderIdAsync(request.OrderId);

            if (payment == null)
                throw new Exception("Payment not found");

            if (payment.Method != PaymentMethod.COD)
                throw new Exception("Only COD payments can be confirmed via cash receipt");

            if (payment.Status != PaymentStatus.Pending)
                return true;

            payment.Status = PaymentStatus.Paid;
            payment.PaidAt = DateTime.UtcNow;

            _repo.Update(payment);
            await _repo.SaveChangesAsync();

            var order = await _orderClient.GetOrderById(payment.OrderId);

            await _eventBus.PublishAsync(new PaymentSucceededEvent
            {
                UserId = order?.UserId,
                OrderId = payment.OrderId,
                PaymentId = payment.Id,
                Items = order?.Items ?? new List<OrderItemDto>()
            });

            try
            {
                await _orderClient.UpdateOrderPaymentStatus(payment.OrderId, "PAID");
            }
            catch
            {
                // ignore order update failure
            }

            return true;
        }
    }
}
