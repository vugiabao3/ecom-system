using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ShippingService.Application.Interfaces;


namespace ShippingService.Application.Shipments.Commands.UpdateShipmentStatus
{
    public class UpdateShipmentStatusHandler
        : IRequestHandler<UpdateShipmentStatusCommand, bool>
    {
        private readonly IShipmentRepository _repo;

        public UpdateShipmentStatusHandler(IShipmentRepository repo)
        {
            _repo = repo;
        }

        public async Task<bool> Handle(UpdateShipmentStatusCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("🔥 UpdateShipmentStatusHandler CALLED");

            var shipment = await _repo.GetByIdAsync(request.ShipmentId);

            if (shipment == null)
            {
                Console.WriteLine("❌ Shipment NOT FOUND");
                return false;
            }

            Console.WriteLine($"👉 Current Status: {shipment.Status}");
            Console.WriteLine($"👉 Request Status: {request.Status}");

            // 🔥 RULE 1: CREATED → DELIVERING
            if (request.Status == "DELIVERING")
            {
                if (shipment.Status != "CREATED")
                {
                    Console.WriteLine("❌ INVALID: phải từ CREATED → DELIVERING");
                    return false;
                }
            }

            // 🔥 RULE 2: DELIVERING → DELIVERED
            else if (request.Status == "DELIVERED")
            {
                if (shipment.Status != "DELIVERING")
                {
                    Console.WriteLine("❌ INVALID: phải từ DELIVERING → DELIVERED");
                    return false;
                }
            }

            // ❌ STATUS KHÔNG HỢP LỆ
            else
            {
                Console.WriteLine("❌ STATUS KHÔNG HỢP LỆ");
                return false;
            }

            // 🔥 UPDATE
            shipment.Status = request.Status;

            await _repo.SaveChangesAsync();

            Console.WriteLine($"✅ Updated status → {shipment.Status}");

            return true;
        }
    }
}