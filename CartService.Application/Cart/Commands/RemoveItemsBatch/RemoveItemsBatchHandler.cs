using CartService.Application.Interfaces;
using MediatR;
namespace CartService.Application.Cart.Commands.RemoveItemsBatch
{
    public class RemoveItemsBatchHandler : IRequestHandler<RemoveItemsBatchCommand, RemoveItemsBatchResponse>
    {
        private readonly ICartRepository _repo;
        private readonly ICurrentUserService _user;
        public RemoveItemsBatchHandler(ICartRepository repo, ICurrentUserService user) { _repo = repo; _user = user; }
        public async Task<RemoveItemsBatchResponse> Handle(RemoveItemsBatchCommand request, CancellationToken ct)
        {
            foreach (var id in request.ItemIds)
            {
                var item = await _repo.GetItemByIdAsync(_user.UserId, id);
                if (item == null) throw new Exception("Item not found");
                _repo.Update(item);
                await _repo.DeleteAsync(item);
            }
            await _repo.SaveChangesAsync();
            return new RemoveItemsBatchResponse { Message = "Batch removed" };
        }
    }
}