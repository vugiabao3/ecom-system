using InventoryService.Application.Inventory.Command.ReserveStock;
using InventoryService.Application.Inventory.Commands.AddStock;
using InventoryService.Application.Inventory.Queries;
using InventoryService.Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace InventoryService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔥 ADD STOCK
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStock(AddStockCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> Get(Guid productId)
        {
            var result = await _mediator.Send(
                new GetInventoryByProductIdQuery(productId));

            return Ok(result);
        }
        // 🔥 RESERVE STOCK
        [HttpPost("reserve")]
        public async Task<IActionResult> Reserve(
            ReserveStockCommand command)
        {
            var result =
                await _mediator.Send(command);

            return Ok(result);
        }
    }
}