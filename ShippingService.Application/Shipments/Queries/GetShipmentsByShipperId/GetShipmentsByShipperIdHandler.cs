using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShippingService.Application.Interfaces;
using ShippingService.Application.DTOs;
using ShippingService.Domain.Entities;

namespace ShippingService.Application.Shipments.Queries.GetShipmentsByShipperId
{
    public class GetShipmentsByShipperIdHandler : IRequestHandler<GetShipmentsByShipperIdQuery, List<ShipmentWithOrderDto>>
    {
        private readonly IShipmentRepository _repo;
        private readonly IOrderServiceClient _orderClient;

        public GetShipmentsByShipperIdHandler(IShipmentRepository repo, IOrderServiceClient orderClient)
        {
            _repo = repo;
            _orderClient = orderClient;
        }

        public async Task<List<ShipmentWithOrderDto>> Handle(GetShipmentsByShipperIdQuery request, CancellationToken cancellationToken)
        {
            var shipments = await _repo.GetByShipperIdAsync(request.ShipperId);
            var result = new List<ShipmentWithOrderDto>();

            foreach (var s in shipments)
            {
                OrderDto? order = null;
                try
                {
                    order = await _orderClient.GetOrder(s.OrderId);
                }
                catch
                {
                    // ignore
                }

                result.Add(new ShipmentWithOrderDto
                {
                    Id = s.Id,
                    OrderId = s.OrderId,
                    Status = s.Status.ToString(),
                    ShipperId = s.ShipperId,
                    ReceiverName = s.ReceiverName,
                    Phone = s.Phone,
                    Address = s.Address,
                    TrackingCode = s.TrackingCode,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt,
                    DeliveredAt = s.DeliveredAt,
                    FailureReason = s.FailureReason,
                    PaymentMethod = order?.PaymentMethod,
                    PaymentStatus = order?.PaymentStatus
                });
            }

            return result;
        }
    }
}
