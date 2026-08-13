using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace CartService.Application.Cart.Commands.AddToCart
{
    public class AddToCartCommand : IRequest<AddToCartResponse>
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
