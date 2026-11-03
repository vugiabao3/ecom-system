using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Queries.GetItemAvailability
{
    public class GetItemAvailabilityHandler : IRequestHandler<GetItemAvailabilityQuery, GetItemAvailabilityResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user; private readonly IProductServiceClient _product;
        public GetItemAvailabilityHandler(ICartRepository repo, ICurrentUserService user, IProductServiceClient product){_repo=repo;_user=user;_product=product;}
        public async Task<GetItemAvailabilityResponse> Handle(GetItemAvailabilityQuery r, CancellationToken ct)
        {
            var item = await _repo.GetItemByIdAsync(_user.UserId, r.ItemId);
            if(item==null) throw new Exception("Item not found");
            var p = await _product.GetProductById(item.ProductId);
            if(p==null) return new GetItemAvailabilityResponse{IsAvailable=false, Reason="Product not found"};
            return new GetItemAvailabilityResponse{IsAvailable=true, Reason="Available"};
        }
    }
}
