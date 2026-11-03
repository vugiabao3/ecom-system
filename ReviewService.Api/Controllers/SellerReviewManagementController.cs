using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api")]
public class SellerReviewManagementController : ControllerBase
{
    [HttpGet("seller/reviews/without-response")]
    public IActionResult WithoutResponse() => Ok(new List<object>());
}
