using MediatR;
namespace CartService.Application.Cart.Queries.GetItemAvailability
{
    public class GetItemAvailabilityQuery : IRequest<GetItemAvailabilityResponse> { public Guid ItemId { get; set; } }
    public class GetItemAvailabilityResponse { public bool IsAvailable { get; set; } public string Reason { get; set; } = string.Empty; }
}