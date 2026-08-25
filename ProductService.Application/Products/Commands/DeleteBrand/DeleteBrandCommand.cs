using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ProductService.Application.Products.Commands.DeleteBrand
{
    public class DeleteBrandCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
