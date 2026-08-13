using ShippingService.Application.DTOs;
using ShippingService.Application.Interfaces;
using ShippingService.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
namespace ShippingService.Infrastructure.Service
{
    public class OrderServiceClient : IOrderServiceClient
    {
        private readonly HttpClient _http;
        private readonly ServiceTokenGenerator _tokenGenerator;

        public OrderServiceClient(HttpClient http, ServiceTokenGenerator tokenGenerator)
        {
            _http = http;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<OrderDto> GetOrder(Guid orderId)
        {
            var token = _tokenGenerator.GenerateToken();

            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await _http.GetAsync($"http://orderservice:8080/api/orders/{orderId}");

            if (!response.IsSuccessStatusCode)
                throw new Exception("Cannot get order from OrderService");

            return await response.Content.ReadFromJsonAsync<OrderDto>();
        }
    }
}
