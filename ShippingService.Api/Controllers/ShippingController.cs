using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingService.Application.Interfaces;
using ShippingService.Application.Shipments.Commands.UpdateShipmentStatus;
using EcomSystem.Contracts.Enums;

namespace ShippingService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IShipmentRepository _repo;

        public ShippingController(IMediator mediator, IShipmentRepository repo)
        {
            _mediator = mediator;
            _repo = repo;
        }

        // 🔥 FLOW 2
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/start-delivery")]
        public async Task<IActionResult> StartDelivery(Guid id)
        {
            var result = await _mediator.Send(new UpdateShipmentStatusCommand
            {
                ShipmentId = id,
                Status = ShipmentStatus.Delivering
            });

            if (!result)
                return BadRequest("Cannot update status");

            return Ok("🚚 Shipment is now DELIVERING");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> Complete(Guid id)
        {
            var result = await _mediator.Send(new UpdateShipmentStatusCommand
            {
                ShipmentId = id,
                Status = ShipmentStatus.Delivered
            });

            if (!result)
                return BadRequest("Cannot complete shipment");

            return Ok("📦 Shipment DELIVERED");
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("by-order/{orderId}")]
        public async Task<IActionResult> GetByOrder(Guid orderId)
        {
            var shipment =
                await _repo.GetByOrderIdAsync(orderId);

            if (shipment == null)
                return NotFound("Shipment not found");

            return Ok(shipment);
        }

    }
}
