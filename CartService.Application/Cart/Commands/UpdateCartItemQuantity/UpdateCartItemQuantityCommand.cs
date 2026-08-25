using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartService.Application.Cart.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommand : IRequest<UpdateCartItemQuantityResponse>
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
