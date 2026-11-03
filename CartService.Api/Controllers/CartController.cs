using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using CartService.Application.Cart.Commands.AddToCart;
using CartService.Application.Cart.Queries.GetCart;
using CartService.Application.Cart.Commands.RemoveItem;
using CartService.Application.Cart.Commands.ClearCart;
using CartService.Application.Cart.Commands.UpdateCartItemQuantity;
using CartService.Application.Cart.Queries.GetCartSummary;
using CartService.Application.Cart.Queries.GetCartCount;
using CartService.Application.Cart.Queries.GetCartShopItems;
using CartService.Application.Cart.Commands.SelectCartShop;
using CartService.Application.Cart.Commands.UnselectCartShop;
using CartService.Application.Cart.Commands.RemoveCartShopItems;
using CartService.Application.Cart.Queries.GetCartShopSummary;
using CartService.Application.Cart.Commands.ValidateCart;
using CartService.Application.Cart.Commands.ValidateSelectedCart;
using CartService.Application.Cart.Queries.GetItemAvailability;
using CartService.Application.Cart.Commands.RefreshCart;
using CartService.Application.Cart.Commands.RefreshCartItems;
using CartService.Application.Cart.Queries.GetInvalidItems;
using CartService.Application.Cart.Commands.RemoveInvalidItems;
using CartService.Application.Cart.Queries.GetCartPricing;
using CartService.Application.Cart.Queries.GetSelectedCartPricing;
using CartService.Application.Cart.Queries.GetShopCartPricing;
using CartService.Application.Cart.Commands.RecalculateCart;
using CartService.Application.Cart.Queries.GetCartDiscounts;
using CartService.Application.Cart.Commands.PreviewCartDiscounts;
using CartService.Application.Cart.Queries.GetAvailableVouchers;
using CartService.Application.Cart.Commands.ApplyVoucher;
using CartService.Application.Cart.Commands.RemoveVoucher;
using CartService.Application.Cart.Queries.GetAppliedVouchers;
using CartService.Application.Cart.Commands.PreviewVoucher;
using CartService.Application.Cart.Commands.SyncCart;
using CartService.Application.Cart.Queries.GetSyncStatus;
using CartService.Application.Cart.Commands.MergeCart;
using CartService.Application.Cart.Queries.GetCartDevices;
using CartService.Application.Cart.Commands.RemoveCartDevice;
using CartService.Application.Cart.Queries.GetCartHistory;
using CartService.Application.Cart.Queries.GetRecentlyRemoved;
using CartService.Application.Cart.Commands.RestoreCartItem;
using CartService.Application.Cart.Commands.Reorder;
using CartService.Application.Cart.Commands.CheckoutPreview;
using CartService.Application.Cart.Commands.CheckoutValidate;
using CartService.Application.Cart.Commands.CheckoutPrepare;
using CartService.Application.Cart.Queries.CheckoutSummary;
using CartService.Application.Cart.Queries.GetShopVouchers;
using CartService.Application.Cart.Queries.GetSelectedCartItems;
using CartService.Application.Cart.Queries.GetSelectedCartSummary;
using CartService.Application.Cart.Queries.GetCartItems;
using CartService.Application.Cart.Queries.GetCartShops;
using CartService.Application.Cart.Queries.GetSelectedCartItems;
using CartService.Application.Cart.Queries.GetSelectedCartSummary;
using CartService.Application.Cart.Queries.GetCartShopItems;
using CartService.Application.Cart.Commands.SelectCartShop;
using CartService.Application.Cart.Commands.UnselectCartShop;
using CartService.Application.Cart.Commands.RemoveCartShopItems;
using CartService.Application.Cart.Queries.GetCartShopSummary;
using CartService.Application.Cart.Commands.ValidateCart;
using CartService.Application.Cart.Commands.ValidateSelectedCart;
using CartService.Application.Cart.Queries.GetItemAvailability;
using CartService.Application.Cart.Commands.RefreshCart;
using CartService.Application.Cart.Commands.RefreshCartItems;
using CartService.Application.Cart.Queries.GetInvalidItems;
using CartService.Application.Cart.Commands.RemoveInvalidItems;
using CartService.Application.Cart.Queries.GetCartPricing;
using CartService.Application.Cart.Queries.GetSelectedCartPricing;
using CartService.Application.Cart.Queries.GetShopCartPricing;
using CartService.Application.Cart.Commands.RecalculateCart;
using CartService.Application.Cart.Queries.GetCartDiscounts;
using CartService.Application.Cart.Commands.PreviewCartDiscounts;
using CartService.Application.Cart.Queries.GetAvailableVouchers;
using CartService.Application.Cart.Commands.ApplyVoucher;
using CartService.Application.Cart.Commands.RemoveVoucher;
using CartService.Application.Cart.Queries.GetAppliedVouchers;
using CartService.Application.Cart.Commands.PreviewVoucher;
using CartService.Application.Cart.Commands.SyncCart;
using CartService.Application.Cart.Queries.GetSyncStatus;
using CartService.Application.Cart.Commands.MergeCart;
using CartService.Application.Cart.Queries.GetCartDevices;
using CartService.Application.Cart.Commands.RemoveCartDevice;
using CartService.Application.Cart.Queries.GetCartHistory;
using CartService.Application.Cart.Queries.GetRecentlyRemoved;
using CartService.Application.Cart.Commands.RestoreCartItem;
using CartService.Application.Cart.Commands.Reorder;
using CartService.Application.Cart.Commands.CheckoutPreview;
using CartService.Application.Cart.Commands.CheckoutValidate;
using CartService.Application.Cart.Commands.CheckoutPrepare;
using CartService.Application.Cart.Queries.CheckoutSummary;
using CartService.Application.Cart.Queries.GetShopVouchers;
using CartService.Application.Cart.Queries.GetCartItem;
using CartService.Application.Cart.Commands.AddToCartBatch;
using CartService.Application.Cart.Commands.UpdateCartSelection;
using CartService.Application.Cart.Commands.RemoveItemsBatch;
using CartService.Application.Cart.Commands.SelectCartItem;
using CartService.Application.Cart.Commands.UnselectCartItem;
using CartService.Application.Cart.Commands.SelectAllCart;
using CartService.Application.Cart.Commands.UnselectAllCart;
using CartService.Application.Cart.Commands.UpdateCartSelection;
using CartService.Application.Cart.Commands.UnselectCartItem;
using CartService.Application.Cart.Commands.SelectAllCart;
using CartService.Application.Cart.Commands.UnselectAllCart;

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

        [HttpPost]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> AddToCart(AddToCartCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetCart()
        {
            var result = await _mediator.Send(new GetCartQuery());
            return Ok(result);
        }

        [HttpPut("items/{itemId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UpdateCartItem(Guid itemId, UpdateCartItemQuantityCommand command)
        {
            command.ItemId = itemId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("items/{itemId}/quantity")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UpdateQuantity(Guid itemId, UpdateCartItemQuantityCommand command)
        {
            command.ItemId = itemId;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("items/{itemId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RemoveItem(Guid itemId)
        {
            var result = await _mediator.Send(new RemoveItemCommand { ItemId = itemId });
            return Ok(result);
        }

        [HttpDelete("clear")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ClearCart()
        {
            var result = await _mediator.Send(new ClearCartCommand());
            return Ok(result);
        }
        [HttpGet("summary")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetSummary()
        {
            var result = await _mediator.Send(new GetCartSummaryQuery());
            return Ok(result);
        }

        [HttpGet("count")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetCount()
        {
            var result = await _mediator.Send(new GetCartCountQuery());
            return Ok(result);
        }

        [HttpGet("items")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetItems()
        {
            var result = await _mediator.Send(new GetCartItemsQuery());
            return Ok(result);
        }

        [HttpGet("items/{itemId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetItem(Guid itemId)
        {
            var result = await _mediator.Send(new GetCartItemQuery { ItemId = itemId });
            return Ok(result);
        }

        [HttpPost("items/batch")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> AddBatch(AddToCartBatchCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("items/batch")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RemoveBatch(RemoveItemsBatchCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpPatch("items/{itemId}/select")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> SelectItem(Guid itemId)
        {
            var result = await _mediator.Send(new SelectCartItemCommand { ItemId = itemId });
            return Ok(result);
        }

        [HttpPatch("items/{itemId}/unselect")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UnselectItem(Guid itemId)
        {
            var result = await _mediator.Send(new UnselectCartItemCommand { ItemId = itemId });
            return Ok(result);
        }

        [HttpPatch("select-all")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> SelectAll()
        {
            var result = await _mediator.Send(new SelectAllCartCommand());
            return Ok(result);
        }

        [HttpPatch("unselect-all")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UnselectAll()
        {
            var result = await _mediator.Send(new UnselectAllCartCommand());
            return Ok(result);
        }

        [HttpPost("selection")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UpdateSelection(UpdateCartSelectionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("selected")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetSelected()
        {
            var result = await _mediator.Send(new GetSelectedCartItemsQuery());
            return Ok(result);
        }

        [HttpGet("selected/summary")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetSelectedSummary()
        {
            var result = await _mediator.Send(new GetSelectedCartSummaryQuery());
            return Ok(result);
        }

        [HttpGet("shops")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetShops()
        {
            var result = await _mediator.Send(new GetCartShopsQuery());
            return Ok(result);
        }

        [HttpGet("shops/{sellerId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetShopItems(Guid sellerId)
        {
            var result = await _mediator.Send(new GetCartShopItemsQuery { SellerId = sellerId });
            return Ok(result);
        }

        [HttpPatch("shops/{sellerId}/select")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> SelectShop(Guid sellerId)
        {
            var result = await _mediator.Send(new SelectCartShopCommand { SellerId = sellerId });
            return Ok(result);
        }
        [HttpPatch("shops/{sellerId}/unselect")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> UnselectShop(Guid sellerId)
        {
            var result = await _mediator.Send(new UnselectCartShopCommand { SellerId = sellerId });
            return Ok(result);
        }

        [HttpGet("shops/{sellerId}/summary")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetShopSummary(Guid sellerId)
        {
            var result = await _mediator.Send(new GetCartShopSummaryQuery { SellerId = sellerId });
            return Ok(result);
        }

        [HttpDelete("shops/{sellerId}/items")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RemoveShopItems(Guid sellerId)
        {
            var result = await _mediator.Send(new RemoveCartShopItemsCommand { SellerId = sellerId });
            return Ok(result);
        }

        [HttpPost("validate")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ValidateCart()
        {
            var result = await _mediator.Send(new ValidateCartCommand());
            return Ok(result);
        }

        [HttpPost("validate/selected")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ValidateSelected()
        {
            var result = await _mediator.Send(new ValidateSelectedCartCommand());
            return Ok(result);
        }
        [HttpGet("items/{itemId}/availability")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetAvailability(Guid itemId)
        {
            var result = await _mediator.Send(new GetItemAvailabilityQuery { ItemId = itemId });
            return Ok(result);
        }

        [HttpPost("refresh")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RefreshCart()
        {
            var result = await _mediator.Send(new RefreshCartCommand());
            return Ok(result);
        }

        [HttpPost("items/refresh")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RefreshItems(RefreshCartItemsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("invalid-items")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetInvalidItems()
        {
            var result = await _mediator.Send(new GetInvalidItemsQuery());
            return Ok(result);
        }

        [HttpDelete("invalid-items")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RemoveInvalidItems()
        {
            var result = await _mediator.Send(new RemoveInvalidItemsCommand());
            return Ok(result);
        }
        [HttpGet("pricing")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetPricing()
        {
            var result = await _mediator.Send(new GetCartPricingQuery());
            return Ok(result);
        }

        [HttpGet("selected/pricing")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetSelectedPricing()
        {
            var result = await _mediator.Send(new GetSelectedCartPricingQuery());
            return Ok(result);
        }

        [HttpGet("shops/{sellerId}/pricing")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetShopPricing(Guid sellerId)
        {
            var result = await _mediator.Send(new GetShopCartPricingQuery { SellerId = sellerId });
            return Ok(result);
        }

        [HttpPost("recalculate")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Recalculate()
        {
            var result = await _mediator.Send(new RecalculateCartCommand());
            return Ok(result);
        }

        [HttpGet("discounts")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetDiscounts()
        {
            var result = await _mediator.Send(new GetCartDiscountsQuery());
            return Ok(result);
        }

        [HttpPost("discounts/preview")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> PreviewDiscounts(PreviewCartDiscountsCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet("vouchers/available")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetAvailableVouchers()
        {
            var result = await _mediator.Send(new GetAvailableVouchersQuery());
            return Ok(result);
        }

        [HttpPost("vouchers/apply")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> ApplyVoucher(ApplyVoucherCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("vouchers/{voucherId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RemoveVoucher(Guid voucherId)
        {
            var result = await _mediator.Send(new RemoveVoucherCommand { VoucherId = voucherId });
            return Ok(result);
        }

        [HttpGet("vouchers/applied")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetAppliedVouchers()
        {
            var result = await _mediator.Send(new GetAppliedVouchersQuery());
            return Ok(result);
        }

        [HttpPost("vouchers/preview")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> PreviewVoucher(PreviewVoucherCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("shops/{sellerId}/vouchers")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetShopVouchers(Guid sellerId)
        {
            var result = await _mediator.Send(new GetShopVouchersQuery { SellerId = sellerId });
            return Ok(result);
        }
        [HttpPost("sync")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Sync()
        {
            var result = await _mediator.Send(new SyncCartCommand());
            return Ok(result);
        }

        [HttpGet("sync/status")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> SyncStatus()
        {
            var result = await _mediator.Send(new GetSyncStatusQuery());
            return Ok(result);
        }

        [HttpPost("merge")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Merge()
        {
            var result = await _mediator.Send(new MergeCartCommand());
            return Ok(result);
        }

        [HttpGet("devices")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetDevices()
        {
            var result = await _mediator.Send(new GetCartDevicesQuery());
            return Ok(result);
        }

        [HttpDelete("devices/{deviceId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RemoveDevice(string deviceId)
        {
            var result = await _mediator.Send(new RemoveCartDeviceCommand { DeviceId = deviceId });
            return Ok(result);
        }
        [HttpGet("history")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetHistory()
        {
            var result = await _mediator.Send(new GetCartHistoryQuery());
            return Ok(result);
        }

        [HttpGet("recently-removed")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> GetRecentlyRemoved()
        {
            var result = await _mediator.Send(new GetRecentlyRemovedQuery());
            return Ok(result);
        }

        [HttpPost("items/{itemId}/restore")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> RestoreItem(Guid itemId)
        {
            var result = await _mediator.Send(new RestoreCartItemCommand { ItemId = itemId });
            return Ok(result);
        }

        [HttpPost("reorder/{orderId}")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> Reorder(Guid orderId)
        {
            var result = await _mediator.Send(new ReorderCommand { OrderId = orderId });
            return Ok(result);
        }
        [HttpPost("checkout/preview")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CheckoutPreview(CheckoutPreviewCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("checkout/validate")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CheckoutValidate(CheckoutValidateCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("checkout/prepare")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CheckoutPrepare(CheckoutPrepareCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("checkout/summary")]
        [Authorize(Policy = "UserOnly")]
        public async Task<IActionResult> CheckoutSummary()
        {
            var result = await _mediator.Send(new CheckoutSummaryQuery());
            return Ok(result);
        }
    }
}