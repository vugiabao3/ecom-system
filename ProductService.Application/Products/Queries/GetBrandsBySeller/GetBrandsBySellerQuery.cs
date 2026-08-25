using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductService.Application.Interfaces;
using ProductService.Domain.Entities;

namespace ProductService.Application.Products.Queries.GetBrandsBySeller
{
    public class GetBrandsBySellerQuery : IRequest<List<Brand>>
    {
        public Guid SellerId { get; set; }
    }
}
