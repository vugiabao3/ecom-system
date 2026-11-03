using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.ValidateSelectedCart
{
    public class ValidateSelectedCartHandler : IRequestHandler<ValidateSelectedCartCommand, ValidateSelectedCartResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user; private readonly IProductServiceClient _product;
        public ValidateSelectedCartHandler(ICartRepository repo, ICurrentUserService user, IProductServiceClient product){_repo=repo;_user=user;_product=product;}
        public async Task<ValidateSelectedCartResponse> Handle(ValidateSelectedCartCommand r, CancellationToken ct)
        {
            var items = (await _repo.GetByUserIdAsync(_user.UserId)).Where(i=>i.IsSelected).ToList();
            bool valid = true;
            foreach(var i in items)
            {
                var p = await _product.GetProductById(i.ProductId);
                if(p==null){ valid=false; i.IsValid=false; _repo.Update(i);}
            }
            await _repo.SaveChangesAsync();
            return new ValidateSelectedCartResponse{IsValid=valid, Message=valid?"Valid":"Invalid selected items"};
        }
    }
}
