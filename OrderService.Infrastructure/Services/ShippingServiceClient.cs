using System;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services
{
    public class ShippingServiceClient : IShippingServiceClient
    {
        private readonly HttpClient _http;

        public ShippingServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<ShippingInfoDto?> GetShippingByOrderId(Guid orderId)
        {
            try
            {
                var response = await _http.GetAsync($"http://shippingservice:8080/api/Shipping/by-order/{orderId}");
                if (!response.IsSuccessStatusCode)
                    return null;

                return await response.Content.ReadFromJsonAsync<ShippingInfoDto>();
            }
            catch
            {
                // Shipping synchronization is best-effort: an unavailable ShippingService
                // must not break order detail retrieval.
                return null;
            }
        }
    }
}
