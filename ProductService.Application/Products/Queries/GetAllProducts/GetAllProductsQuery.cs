using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
namespace ProductService.Application.Products.Queries.GetAllProducts
{
    public class GetAllProductsQuery : IRequest<GetAllProductsResponse>
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 10;

        public int? Category { get; set; }
        public string? Sort { get; set; }
    }
}
