using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using PaymentService.Application.Interfaces;
using PaymentService.Domain.Entities;
using PaymentService.Application.Events;

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
            // 🔥 DEBUG
            Console.WriteLine("====== PAYMENT DEBUG ======");
            Console.WriteLine($"Request OrderId: {request.OrderId}");

            // 🔥 1. CHECK ORDER TỒN TẠI
            var order = await _orderClient.GetOrderById(request.OrderId);

            Console.WriteLine($"Response OrderId: {order?.Id}");
            Console.WriteLine($"Order Status: {order?.Status}");
            Console.WriteLine("===========================");
            if (order == null)
                throw new Exception("Order not found");

            // 🔥 2. CHECK ORDER STATUS
            if (order.Status != "PENDING")
                throw new Exception("Order already processed");

            // 🔥 3. CHECK AMOUNT
            //if (order.TotalPrice != request.Amount)
            //    throw new Exception("Invalid amount");

            // 🔥 4. fake payment logic
            var success = true;

            var payment = new Payment
            {
                Id = Guid.NewGuid(),
                OrderId = request.OrderId,
                //Amount = request.Amount,
                Status = success ? "SUCCESS" : "FAILED",
                CreatedAt = DateTime.UtcNow
            };

            await _repo.AddAsync(payment);
            await _repo.SaveChangesAsync();

            // 🔥 5. publish event
            if (success)
            {
                Console.WriteLine("🔥 PUBLISH PAYMENT SUCCESS EVENT");
                await _eventBus.PublishAsync(new PaymentSucceededEvent
                {
                    //UserId = request.UserId,              // 🔥 THÊM CÁI NÀY
                    OrderId = request.OrderId,
                    PaymentId = payment.Id,
                    //Items = request.Items
                });
                Console.WriteLine("✅ EVENT PUBLISHED");

            }
            else
            {
                await _eventBus.PublishAsync(new PaymentFailedEvent
                {
                    OrderId = request.OrderId
                });
            }

            return new ProcessPaymentResponse
            {
                PaymentId = payment.Id,
                Status = payment.Status
            };
        }
    }
}
