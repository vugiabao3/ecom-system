using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CartService.Application.DTOs;
namespace CartService.Application.Interfaces
{
    public interface IProductServiceClient
    {
        Task<ProductDto> GetProductById(Guid id);
        Task<List<ProductDto>> GetProductsByIds(List<Guid> ids);
    }
}
