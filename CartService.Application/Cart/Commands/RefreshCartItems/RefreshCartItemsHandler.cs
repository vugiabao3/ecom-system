using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.RefreshCartItems
{
    public class RefreshCartItemsHandler : IRequestHandler<RefreshCartItemsCommand, RefreshCartItemsResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user; private readonly IProductServiceClient _product;
        public RefreshCartItemsHandler(ICartRepository repo, ICurrentUserService user, IProductServiceClient product){_repo=repo;_user=user;_product=product;}
        public async Task<RefreshCartItemsResponse> Handle(RefreshCartItemsCommand r, CancellationToken ct)
        {
            foreach(var id in r.ItemIds)
            {
                var item = await _repo.GetItemByIdAsync(_user.UserId, id);
                if(item==null) continue;
                var p = await _product.GetProductById(item.ProductId);
                if(p!=null){ item.ProductName=p.Name; item.PriceSnapshot=p.Price; _repo.Update(item);}
            }
            await _repo.SaveChangesAsync();
            return new RefreshCartItemsResponse{Message="Refreshed"};
        }
    }
}
