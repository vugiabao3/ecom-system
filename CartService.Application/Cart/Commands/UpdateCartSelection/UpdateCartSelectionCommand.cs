using MediatR;
namespace CartService.Application.Cart.Commands.UpdateCartSelection
{
    public class UpdateCartSelectionCommand : IRequest<UpdateCartSelectionResponse>
    {
        public List<SelectionEntry> Selections { get; set; } = new();
    }
    public class SelectionEntry { public Guid ItemId { get; set; } public bool IsSelected { get; set; } }
    public class UpdateCartSelectionResponse { public string Message { get; set; } = string.Empty; }
}