using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryService.Api.Controllers
{
    [ApiController]
    [Route("api/seller/inventory")]
    public class SellerInventoryController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public IActionResult List() => Ok(new List<object>());

        [HttpGet("summary")]
        [Authorize]
        public IActionResult Summary() => Ok(new { onHand = 0, reserved = 0, available = 0 });

        [HttpGet("{productId}")]
        [Authorize]
        public IActionResult Product(Guid productId) => Ok(new { productId });

        [HttpGet("variants/{variantId}")]
        [Authorize]
        public IActionResult Variant(Guid variantId) => Ok(new { variantId });

        [HttpGet("low-stock")]
        [Authorize]
        public IActionResult LowStock() => Ok(new List<object>());

        [HttpGet("out-of-stock")]
        [Authorize]
        public IActionResult OutOfStock() => Ok(new List<object>());

        [HttpPost("add-stock")]
        [Authorize]
        public IActionResult AddStock() => Ok(new { message = "Stock added" });

        [HttpPatch("{productId}/threshold")]
        [Authorize]
        public IActionResult Threshold(Guid productId) => Ok(new { productId, message = "Threshold updated" });

        [HttpGet("stock-movements")]
        [Authorize]
        public IActionResult StockMovements() => Ok(new List<object>());

        [HttpGet("statistics")]
        [Authorize]
        public IActionResult Statistics() => Ok(new { totalProducts = 0, totalOnHand = 0, totalReserved = 0, totalAvailable = 0 });

        [HttpGet("alerts")]
        [Authorize]
        public IActionResult Alerts() => Ok(new List<object>());

        [HttpGet("reports")]
        [Authorize]
        public IActionResult Reports() => Ok(new { message = "Seller reports" });
    }
}
