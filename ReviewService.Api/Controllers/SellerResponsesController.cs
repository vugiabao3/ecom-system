using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/reviews/{reviewId}/seller-response")]
public class SellerResponsesController : ControllerBase
{
    [HttpPost]
    public IActionResult Create(Guid reviewId) => Ok(new { reviewId, responseId = Guid.NewGuid() });
    [HttpGet]
    public IActionResult Get(Guid reviewId) => Ok(new { reviewId, content = "" });
    [HttpPut]
    public IActionResult Edit(Guid reviewId) => Ok(new { reviewId, message = "Updated" });
    [HttpDelete]
    public IActionResult Delete(Guid reviewId) => Ok(new { reviewId, message = "Deleted" });
}
