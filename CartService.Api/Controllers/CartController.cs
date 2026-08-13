using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CartService.Application.Cart.Commands.AddToCart;
using CartService.Application.Cart.Queries.GetCart;
using CartService.Application.Cart.Commands.RemoveItem;
using CartService.Application.Cart.Commands.ClearCart;

namespace CartService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CartController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // 🔥 FLOW 1
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(AddToCartCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCart()
        {
            var result = await _mediator.Send(new GetCartQuery());
            return Ok(result);
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> RemoveItem(RemoveItemCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("clear")]
        [Authorize]
        public async Task<IActionResult> ClearCart()
        {
            var result = await _mediator.Send(new ClearCartCommand());
            return Ok(result);
        }
    }
}
