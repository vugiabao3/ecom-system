using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.RefreshCart
{
    public class RefreshCartHandler : IRequestHandler<RefreshCartCommand, RefreshCartResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user; private readonly IProductServiceClient _product;
        public RefreshCartHandler(ICartRepository repo, ICurrentUserService user, IProductServiceClient product){_repo=repo;_user=user;_product=product;}
        public async Task<RefreshCartResponse> Handle(RefreshCartCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            foreach(var i in items)
            {
                var p = await _product.GetProductById(i.ProductId);
                if(p!=null){ i.ProductName=p.Name; i.PriceSnapshot=p.Price; _repo.Update(i);}
            }
            await _repo.SaveChangesAsync();
            return new RefreshCartResponse{Message="Refreshed"};
        }
    }
}
