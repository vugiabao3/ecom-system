using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services
{
    public class InventoryServiceClient : IInventoryServiceClient
    {
        private readonly HttpClient _http;

        public InventoryServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<InventoryDto> GetInventoryByProductId(Guid productId)
        {
            var response = await _http.GetAsync($"http://inventoryservice:8080/api/Inventory/{productId}");
            if (!response.IsSuccessStatusCode)
                return new InventoryDto { ProductId = productId, Available = 0, Reserved = 0 };
            return await response.Content.ReadFromJsonAsync<InventoryDto>() ?? new InventoryDto { ProductId = productId };
        }

        public async Task<ReserveStockResponse> ReserveStock(Guid productId, int quantity)
        {
            var response = await _http.PostAsJsonAsync(
                "http://inventoryservice:8080/api/Inventory/reserve",
                new { productId, quantity });

            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<ReserveStockResponse>(json) ?? new ReserveStockResponse { ProductId = productId };
        }
    }
}
