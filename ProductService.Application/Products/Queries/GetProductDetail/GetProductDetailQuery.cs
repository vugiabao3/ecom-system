using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Products.Queries.GetProductDetail
{
    public class GetProductDetailQuery : IRequest<GetProductDetailResponse>
    {
        public Guid Id { get; set; }

        public GetProductDetailQuery(Guid id)
        {
            Id = id;
        }
    }
}
