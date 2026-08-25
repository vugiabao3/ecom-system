using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Application.Events;
using EcomSystem.Contracts.Enums;

namespace PaymentService.Application.Payments.Commands.ProcessPayment
{
    public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, ProcessPaymentResponse>
    {
        private readonly IPaymentRepository _repo;
        private readonly IEventBus _eventBus;
        private readonly IOrderServiceClient _orderClient;

        public ProcessPaymentHandler(IPaymentRepository repo, IEventBus eventBus, IOrderServiceClient orderClient)
        {
            _repo = repo;
            _eventBus = eventBus;
            _orderClient = orderClient;
        }

        public async Task<ProcessPaymentResponse> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderClient.GetOrderById(request.OrderId);
            if (order == null)
                throw new Exception("Order not found");

            if (order.Status != "PENDING" && order.Status != OrderStatus.Pending.ToString())
                throw new Exception("Order already processed");

            var existing = await _repo.GetByOrderIdAsync(request.OrderId);
            if (existing != null)
            {
                if (existing.Status == PaymentStatus.Paid || existing.Status == PaymentStatus.Failed)
                {
                    return new ProcessPaymentResponse
                    {
                        PaymentId = existing.Id,
                        Status = existing.Status.ToString()
                    };
                }
            }

            var method = Enum.Parse<PaymentMethod>(request.PaymentMethod, true);

            var payment = new Payment
            {
                Id = existing != null ? existing.Id : Guid.NewGuid(),
                OrderId = request.OrderId,
                Amount = order.TotalPrice,
                Status = method == PaymentMethod.COD ? PaymentStatus.Pending : PaymentStatus.Pending,
                Method = method,
                CreatedAt = existing != null ? existing.CreatedAt : DateTime.UtcNow
            };

            if (existing != null)
            {
                existing.Status = payment.Status;
                existing.Method = payment.Method;
                existing.Amount = payment.Amount;
                _repo.Update(existing);
            }
            else
            {
                await _repo.AddAsync(payment);
            }

            await _repo.SaveChangesAsync();

            await _eventBus.PublishAsync(new PaymentPendingEvent
            {
                OrderId = request.OrderId,
                PaymentId = payment.Id,
                PaymentMethod = payment.Method.ToString()
            });

            return new ProcessPaymentResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status.ToString().ToUpper()
            };
        }
    }
}
