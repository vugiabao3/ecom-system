using CartService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CartService.Application.DTOs;
namespace CartService.Infrastructure.Services
{
    public class ProductServiceClient : IProductServiceClient
    {
        private readonly HttpClient _http;

        public ProductServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ProductDto> GetProductById(Guid id)
        {
            return await _http.GetFromJsonAsync<ProductDto>(
                $"http://productservice:8080/api/products/{id}");
        }
        public async Task<List<ProductDto>> GetProductsByIds(List<Guid> ids)
        {
            var response = await _http.PostAsJsonAsync(
                "http://productservice:8080/api/products/batch",
                ids);

            // 🔥 check status trước
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"ProductService error: {error}");
            }

            // 🔥 nếu OK mới parse JSON
            var result = await response.Content.ReadFromJsonAsync<List<ProductDto>>();

            return result ?? new List<ProductDto>();
        }
    }
}
