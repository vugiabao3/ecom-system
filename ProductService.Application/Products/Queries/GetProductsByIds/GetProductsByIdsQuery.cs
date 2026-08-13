using MediatR;
using ProductService.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Queries.GetProductsByIds
{
    public class GetProductsByIdsQuery : IRequest<List<ProductDto>>
    {
        public List<Guid> Ids { get; set; }

        public GetProductsByIdsQuery(List<Guid> ids)
        {
            Ids = ids;
        }
    }
}
