using MediatR;
namespace CartService.Application.Cart.Commands.RemoveItemsBatch
{
    public class RemoveItemsBatchCommand : IRequest<RemoveItemsBatchResponse>
    {
        public List<Guid> ItemIds { get; set; } = new();
    }
    public class RemoveItemsBatchResponse { public string Message { get; set; } = string.Empty; }
}