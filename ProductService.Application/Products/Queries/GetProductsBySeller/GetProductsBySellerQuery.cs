using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Application.DTO;

namespace ProductService.Application.Products.Queries.GetProductsBySeller
{
    public class GetProductsBySellerQuery : IRequest<List<ProductDto>>
    {
        public Guid SellerId { get; set; }
    }
}
