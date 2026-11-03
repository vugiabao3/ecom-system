using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.AddToCartBatch
{
    public class AddToCartBatchHandler : IRequestHandler<AddToCartBatchCommand, AddToCartBatchResponse>
    {
        private readonly ICartRepository _repo;
        private readonly IProductServiceClient _product;
        private readonly ICurrentUserService _user;
        public AddToCartBatchHandler(ICartRepository repo, IProductServiceClient product, ICurrentUserService user) { _repo = repo; _product = product; _user = user; }
        public async Task<AddToCartBatchResponse> Handle(AddToCartBatchCommand request, CancellationToken ct)
        {
            foreach (var entry in request.Items)
            {
                if (entry.Quantity <= 0) throw new Exception("Invalid quantity");
                var product = await _product.GetProductById(entry.ProductId);
                if (product == null) throw new Exception("Product not found");
                var existing = await _repo.GetItemAsync(_user.UserId, product.Id);
                if (existing != null)
                {
                    existing.Quantity += entry.Quantity;
                    _repo.Update(existing);
                }
                else
                {
                    await _repo.AddAsync(new CartService.Domain.Entities.CartItem
                    {
                        Id = Guid.NewGuid(),
                        UserId = _user.UserId,
                        ProductId = product.Id,
                        ProductName = product.Name,
                        Quantity = entry.Quantity,
                        PriceSnapshot = product.Price
                    });
                }
            }
            await _repo.SaveChangesAsync();
            return new AddToCartBatchResponse { Message = "Batch added" };
        }
    }
}