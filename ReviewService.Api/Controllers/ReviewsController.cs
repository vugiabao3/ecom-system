using Microsoft.AspNetCore.Mvc;
namespace ReviewService.Api.Controllers;
[ApiController]
[Route("api/reviews")]
public class ReviewsController : ControllerBase
{
    [HttpPost]
    public IActionResult Create() => Ok(new { id = Guid.NewGuid() });
    [HttpGet]
    public IActionResult List() => Ok(new List<object>());
    [HttpGet("{id}")]
    public IActionResult Get(Guid id) => Ok(new { id });
    [HttpPut("{id}")]
    public IActionResult Edit(Guid id) => Ok(new { id, message = "Updated" });
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id) => Ok(new { id, message = "Deleted" });
}
