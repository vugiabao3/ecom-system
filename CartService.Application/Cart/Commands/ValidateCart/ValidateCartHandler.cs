using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.ValidateCart
{
    public class ValidateCartHandler : IRequestHandler<ValidateCartCommand, ValidateCartResponse>
    {
        private readonly ICartRepository _repo; private readonly ICurrentUserService _user; private readonly IProductServiceClient _product;
        public ValidateCartHandler(ICartRepository repo, ICurrentUserService user, IProductServiceClient product){_repo=repo;_user=user;_product=product;}
        public async Task<ValidateCartResponse> Handle(ValidateCartCommand r, CancellationToken ct)
        {
            var items = await _repo.GetByUserIdAsync(_user.UserId);
            bool valid = true;
            foreach(var i in items)
            {
                var p = await _product.GetProductById(i.ProductId);
                if(p==null){ valid=false; i.IsValid=false; _repo.Update(i);}
            }
            await _repo.SaveChangesAsync();
            return new ValidateCartResponse{IsValid=valid, Message=valid?"Valid":"Invalid items found"};
        }
    }
}
