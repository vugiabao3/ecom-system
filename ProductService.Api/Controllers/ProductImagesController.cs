using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Api.Controllers
{
    [ApiController]
    [Route("api/products/{id}/images")]
    public class ProductImagesController : ControllerBase
    {
        private readonly IProductRepository _productRepo;
        private readonly IProductImageRepository _imageRepo;

        public ProductImagesController(IProductRepository productRepo, IProductImageRepository imageRepo)
        {
            _productRepo = productRepo;
            _imageRepo = imageRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var images = await _imageRepo.GetByProductIdAsync(id);
            return Ok(images);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Upload(Guid id, [FromForm] IFormFile file)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null) return NotFound();
            if (file == null || file.Length == 0) return BadRequest("No file");

            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploads, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var img = new ProductImage
            {
                Id = Guid.NewGuid(),
                ProductId = id,
                ImageUrl = "/uploads/" + fileName,
                IsMain = !(await _imageRepo.GetByProductIdAsync(id)).Any(),
                DisplayOrder = (await _imageRepo.GetByProductIdAsync(id)).Count
            };
            await _imageRepo.AddAsync(img);
            return Ok(img);
        }

        [HttpPut("{imageId}")]
        [Authorize]
        public async Task<IActionResult> Update(Guid id, Guid imageId, [FromBody] ProductImage update)
        {
            var img = await _imageRepo.GetByIdAsync(imageId);
            if (img == null || img.ProductId != id) return NotFound();
            img.ImageUrl = update.ImageUrl ?? img.ImageUrl;
            img.DisplayOrder = update.DisplayOrder;
            await _imageRepo.UpdateAsync(img);
            return Ok(img);
        }

        [HttpDelete("{imageId}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id, Guid imageId)
        {
            var img = await _imageRepo.GetByIdAsync(imageId);
            if (img == null || img.ProductId != id) return NotFound();
            bool wasMain = img.IsMain;
            await _imageRepo.DeleteAsync(imageId);
            if (wasMain)
            {
                var remaining = await _imageRepo.GetByProductIdAsync(id);
                if (remaining.Any())
                {
                    remaining.First().IsMain = true;
                    await _imageRepo.UpdateAsync(remaining.First());
                }
            }
            return NoContent();
        }

        [HttpPatch("reorder")]
        [Authorize]
        public async Task<IActionResult> Reorder(Guid id, [FromBody] List<Guid> imageIds)
        {
            var images = await _imageRepo.GetByProductIdAsync(id);
            for (int i = 0; i < imageIds.Count; i++)
            {
                var img = images.FirstOrDefault(x => x.Id == imageIds[i]);
                if (img != null)
                {
                    img.DisplayOrder = i;
                    await _imageRepo.UpdateAsync(img);
                }
            }
            var updated = await _imageRepo.GetByProductIdAsync(id);
            return Ok(updated);
        }
        [Authorize]
        public async Task<IActionResult> SetPrimary(Guid id, Guid imageId)
        {
            var images = await _imageRepo.GetByProductIdAsync(id);
            foreach (var im in images)
            {
                im.IsMain = im.Id == imageId;
                await _imageRepo.UpdateAsync(im);
            }
            return NoContent();
        }
    }
}