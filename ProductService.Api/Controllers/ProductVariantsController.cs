using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/products/{id}/variants")]
    public class ProductVariantsController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductVariantRepository _variantRepo;

        public ProductVariantsController(IProductRepository productRepo, IProductVariantRepository variantRepo)
        {
            _productRepo = productRepo;
            _variantRepo = variantRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();
            var variants = await _variantRepo.GetByProductIdAsync(id);
            return Ok(variants);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Guid id, [FromBody] ProductVariant variant)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();
            variant.Id = Guid.NewGuid();
            variant.ProductId = id;
            await _variantRepo.AddAsync(variant);
            return CreatedAtAction(nameof(GetById), new { id, variantId = variant.Id }, variant);
        }

        [HttpGet("{variantId}")]
        public async Task<IActionResult> GetById(Guid id, Guid variantId)
        {
            var variant = await _variantRepo.GetByIdAsync(variantId);
            if (variant == null || variant.ProductId != id) return NotFound();
            return Ok(variant);
        }

        [HttpPut("{variantId}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, Guid variantId, [FromBody] ProductVariant variant)
        {
            var existing = await _variantRepo.GetByIdAsync(variantId);
            if (existing == null || existing.ProductId != id) return NotFound();
            existing.SKU = variant.SKU ?? existing.SKU;
            existing.Price = variant.Price;
            await _variantRepo.UpdateAsync(existing);
            return Ok(existing);
        }

        [HttpDelete("{variantId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id, Guid variantId)
        {
            var existing = await _variantRepo.GetByIdAsync(variantId);
            if (existing == null || existing.ProductId != id) return NotFound();
            await _variantRepo.DeleteAsync(variantId);
            return NoContent();
        }

        [HttpPost("batch")]
        [Authorize]
        public async Task<IActionResult> Batch(Guid id, [FromBody] List<ProductVariant> variants)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();
            var created = new List<ProductVariant>();
            foreach (var v in variants)
            {
                v.Id = Guid.NewGuid();
                v.ProductId = id;
                await _variantRepo.AddAsync(v);
                created.Add(v);
            }
            return Ok(created);
        }

        [HttpPatch("{variantId}/status")]
        [Authorize]
        public async Task<IActionResult> PatchStatus(Guid id, Guid variantId, [FromBody] Dictionary<string, bool> body)
        {
            var existing = await _variantRepo.GetByIdAsync(variantId);
            if (existing == null || existing.ProductId != id) return NotFound();
            if (body.TryGetValue("isActive", out var active))
            {
                existing.IsActive = active;
            }
            await _variantRepo.UpdateAsync(existing);
            return Ok(existing);
        }
    }
}