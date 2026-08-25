using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ProductService.Application.Products.Commands.UpdateProduct
{

    public class UpdateProductCommand : IRequest<UpdateProductResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public Guid BrandId { get; set; }
    }
}
