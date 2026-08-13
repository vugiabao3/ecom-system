using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PromotionService.Application.Promotions.Commands.ApplyPromotion;
using PromotionService.Application.Promotions.Commands.CreatePromotion;
using PromotionService.Application.Promotions.Commands.DeletePromotion;
using PromotionService.Application.Promotions.Commands.UpdatePromotion;
using PromotionService.Application.Promotions.Queries.GetAllPromotions;

namespace PromotionService.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromotionController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [Authorize(AuthenticationSchemes = "Internal")]
    [HttpPost("apply")]
    public async Task<IActionResult> Apply([FromBody] ApplyPromotionCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreatePromotion(
    [FromBody] CreatePromotionCommand command)
    {
        var result =
            await _mediator.Send(command);

        return Ok(result);
    }
    [HttpPut]
    [Route("update")]
    public async Task<IActionResult>
    UpdatePromotion(
        UpdatePromotionCommand command)
    {
        var result =
            await _mediator
                .Send(command);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id}")]
    public async Task<IActionResult>
    DeletePromotion(Guid id)
    {
        var result =
            await _mediator
                .Send(
                    new DeletePromotionCommand
                    {
                        Id = id
                    });

        return Ok(result);
    }


    [HttpGet]
    [Route("all")]
    public async Task<IActionResult>
    GetAllPromotions()
    {
        var result =
            await _mediator
                .Send(
                    new GetAllPromotionsQuery());

        return Ok(result);
    }
}