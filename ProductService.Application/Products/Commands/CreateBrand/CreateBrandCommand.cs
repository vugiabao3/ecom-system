using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace ProductService.Application.Products.Commands.CreateBrand
{
    public class CreateBrandCommand : IRequest<CreateBrandResponse>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
    }
}
