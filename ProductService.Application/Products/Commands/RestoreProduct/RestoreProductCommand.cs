using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Commands.RestoreProduct
{
    public class RestoreProductCommand : IRequest<RestoreProductResponse>
    {
        public Guid Id { get; set; }

        public RestoreProductCommand(Guid id)
        {
            Id = id;
        }
    }
}
