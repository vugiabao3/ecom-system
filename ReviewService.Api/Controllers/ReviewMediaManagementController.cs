using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class ReviewMediaManagementController : ControllerBase
{
    [HttpDelete("reviews/{reviewId}/media/{mediaId}")]
    public IActionResult DeleteMedia(Guid reviewId, Guid mediaId) => Ok(new { reviewId, mediaId, message = "Deleted" });
    [HttpPatch("reviews/{reviewId}/media/reorder")]
    public IActionResult ReorderMedia(Guid reviewId) => Ok(new { reviewId, message = "Reordered" });
    [HttpPost("reviews/media/upload-url")]
    public IActionResult UploadUrl() => Ok(new { uploadUrl = "https://example.com/upload", mediaId = Guid.NewGuid() });
    [HttpPost("reviews/media/{mediaId}/confirm")]
    public IActionResult ConfirmUpload(Guid mediaId) => Ok(new { mediaId, message = "Confirmed" });
    [HttpPost("reviews/{reviewId}/helpful")]
    public IActionResult Helpful(Guid reviewId) => Ok(new { reviewId, helpful = true });
}
