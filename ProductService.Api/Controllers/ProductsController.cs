using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Products.Queries.GetAllProducts;
using ProductService.Application.Products.Queries.GetProductDetail;
using ProductService.Application.Products.Commands.CreateProduct;
using ProductService.Application.Products.Commands.UpdateProduct;
using ProductService.Application.Products.Commands.DeleteProduct;
using ProductService.Application.Products.Commands.CreateBrand;
using ProductService.Application.Products.Commands.UpdateBrand;
using ProductService.Application.Products.Commands.DeleteBrand;
using ProductService.Application.Products.Queries.GetProductsBySeller;
using ProductService.Application.Products.Commands.RestoreProduct;
using ProductService.Application.Products.Commands.DuplicateProduct;
using ProductService.Application.Products.Commands.PatchProduct;
using ProductService.Application.Products.Commands.PatchProductStatus;
using ProductService.Application.Products.Queries.SearchProduct;
using ProductService.Application.Products.Queries.GetProductsByIds;
using ProductService.Application.Products.Queries.GetBrandsBySeller;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpPost("batch")]
        public async Task<IActionResult> GetByIds([FromBody] List<Guid> ids)
        {
            var result = await _mediator.Send(new GetProductsByIdsQuery(ids));
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductDetail(Guid id)
        {
            var result = await _mediator.Send(new GetProductDetailQuery(id));
            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateProduct(CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> PatchProduct(Guid id, PatchProductCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _mediator.Send(new DeleteProductCommand(id));
            return Ok(result);
        }
        [HttpPut("{id}/restore")]
        [Authorize]
        public async Task<IActionResult> RestoreProduct(Guid id)
        {
            var result = await _mediator.Send(new RestoreProductCommand(id));
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] SearchProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("new-arrivals")]
        public async Task<IActionResult> GetNewArrivals([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllProductsQuery { Page = page, PageSize = pageSize, SortBy = "newest" };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("best-selling")]
        public async Task<IActionResult> GetBestSelling([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = new GetAllProductsQuery { Page = page, PageSize = pageSize, SortBy = "newest" };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}/related")]
        public async Task<IActionResult> GetRelated(Guid id)
        {
            var product = await _mediator.Send(new GetProductDetailQuery(id));
            if (product == null) return NotFound();
            var query = new GetAllProductsQuery { Page = 1, PageSize = 10, SortBy = "newest" };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}/recommendations")]
        public async Task<IActionResult> GetRecommendations(Guid id)
        {
            var product = await _mediator.Send(new GetProductDetailQuery(id));
            if (product == null) return NotFound();
            var query = new GetAllProductsQuery { Page = 1, PageSize = 10, SortBy = "newest" };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPatch("{id}/status")]
        [Authorize]
        public async Task<IActionResult> PatchStatus(Guid id, [FromBody] PatchProductStatusCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("{id}/duplicate")]
        [Authorize]
        public async Task<IActionResult> DuplicateProduct(Guid id)
        {
            var result = await _mediator.Send(new DuplicateProductCommand { Id = id });
            return Ok(result);
        }
        [Authorize]
        public async Task<IActionResult> GetProductsBySeller(Guid sellerId)
        {
            var result = await _mediator.Send(new GetProductsBySellerQuery { SellerId = sellerId });
            return Ok(result);
        }

        [HttpPost("brands")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> CreateBrand(CreateBrandCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("brands/{id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> UpdateBrand(Guid id, UpdateBrandCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("brands/{id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> DeleteBrand(Guid id)
        {
            var result = await _mediator.Send(new DeleteBrandCommand { Id = id });
            return Ok(result);
        }

        [HttpGet("brands/seller/{sellerId}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> GetBrandsBySeller(Guid sellerId)
        {
            var result = await _mediator.Send(new GetBrandsBySellerQuery { SellerId = sellerId });
            return Ok(result);
        }
    }
}