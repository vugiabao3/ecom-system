using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartService.Application.DTOs;
namespace CartService.Application.Cart.Queries.GetCart
{
    public class GetCartResponse
    {
        public List<CartItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
