using CartService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace CartService.Infrastructure.Services
{
    public class InventoryServiceClient
        : IInventoryServiceClient
    {
        private readonly HttpClient _httpClient;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public InventoryServiceClient(
            HttpClient httpClient,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        // 🔥 forward JWT
        private void AddJwtHeader()
        {
            var token =
                _httpContextAccessor
                .HttpContext?
                .Request
                .Headers["Authorization"]
                .ToString();

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(token);
            }
        }

        public async Task ReserveStockAsync(
            Guid productId,
            int quantity)
        {
            AddJwtHeader();

            var response =
                await _httpClient.PostAsJsonAsync(
                            "/api/inventory/reserve",
                    new
                    {
                        productId,
                        quantity
                    });

            response.EnsureSuccessStatusCode();
        }

        public async Task ReleaseStockAsync(
            Guid productId,
            int quantity)
        {
            AddJwtHeader();

            var response =
                await _httpClient.PostAsJsonAsync(
                           "/api/inventory/reserve",
                    new
                    {
                        productId,
                        quantity
                    });

            response.EnsureSuccessStatusCode();
        }
    }
}