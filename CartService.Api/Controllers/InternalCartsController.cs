using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CartService.Application.Cart.Queries.InternalCartItems;
using CartService.Application.Cart.Commands.InternalClearCart;
using CartService.Application.Cart.Commands.InternalRemoveCartItemsBatch;
using CartService.Application.Cart.Queries.InternalCartExists;
using CartService.Application.Cart.Queries.InternalCartStatistics;
using CartService.Application.Cart.Queries.InternalCartSummary;

namespace CartService.Api.Controllers
{
    [ApiController]
    [Route("api/internal/carts")]
    public class InternalCartsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public InternalCartsController(IMediator mediator) { _mediator = mediator; }

        [HttpGet("{userId}/summary")]
        [Authorize(Policy = "ServiceOnly")]
        public async Task<IActionResult> Summary(string userId)
        {
            var result = await _mediator.Send(new InternalCartSummaryQuery { UserId = userId });
            return Ok(result);
        }
        [HttpGet("{userId}/items")]
        [Authorize(Policy = "ServiceOnly")]
        public async Task<IActionResult> Items(string userId)
        {
            var result = await _mediator.Send(new InternalCartItemsQuery { UserId = userId });
            return Ok(result);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "ServiceOnly")]
        public async Task<IActionResult> Clear(string userId)
        {
            var result = await _mediator.Send(new InternalClearCartCommand { UserId = userId });
            return Ok(result);
        }

        [HttpPost("{userId}/items/remove-batch")]
        [Authorize(Policy = "ServiceOnly")]
        public async Task<IActionResult> RemoveBatch(string userId, InternalRemoveCartItemsBatchCommand command)
        {
            command.UserId = userId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{userId}/exists")]
        [Authorize(Policy = "ServiceOnly")]
        public async Task<IActionResult> Exists(string userId)
        {
            var result = await _mediator.Send(new InternalCartExistsQuery { UserId = userId });
            return Ok(result);
        }

        [HttpGet("statistics")]
        [Authorize(Policy = "ServiceOnly")]
        public async Task<IActionResult> Statistics()
        {
            var result = await _mediator.Send(new InternalCartStatisticsQuery());
            return Ok(result);
        }
    }
}