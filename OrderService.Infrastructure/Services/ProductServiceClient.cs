using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http.Json;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _http;

        public ProductServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<List<ProductDto>> GetProductsByIds(List<Guid> ids)
        {
            var response = await _http.PostAsJsonAsync(
                "http://productservice:8080/api/products/batch", ids);

            return await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        }
    }
}
