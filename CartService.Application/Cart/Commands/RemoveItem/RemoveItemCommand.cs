using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Cart.Commands.RemoveItem
{
    public class RemoveItemCommand : IRequest<RemoveItemResponse>
    {
        public Guid ProductId { get; set; }
    }
}
