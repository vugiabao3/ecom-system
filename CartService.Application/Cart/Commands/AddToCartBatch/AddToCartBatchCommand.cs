using MediatR;
namespace CartService.Application.Cart.Commands.AddToCartBatch
{
    public class AddToCartBatchCommand : IRequest<AddToCartBatchResponse>
    {
        public List<BatchAddItem> Items { get; set; } = new();
    }
    public class BatchAddItem { public Guid ProductId { get; set; } public int Quantity { get; set; } }
    public class AddToCartBatchResponse { public string Message { get; set; } = string.Empty; }
}