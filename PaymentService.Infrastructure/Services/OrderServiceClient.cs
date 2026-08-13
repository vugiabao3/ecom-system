using PaymentService.Application.DTOs;
using PaymentService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using PaymentService.Infrastructure.Security;

namespace PaymentService.Infrastructure.Services
{
    public class OrderServiceClient : IOrderServiceClient
    {
        private readonly HttpClient _http;
        private readonly InternalTokenGenerator _token;

        public OrderServiceClient(HttpClient http, InternalTokenGenerator token)
        {
            _http = http;
            _token = token;
        }

        public async Task<OrderDto?> GetOrderById(Guid orderId)
        {
            var jwt = _token.Generate(); // 🔥 tự generate internal token

            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"http://orderservice:8080/api/orders/{orderId}"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);

            var response = await _http.SendAsync(request);

            if (!response.IsSuccessStatusCode)
{
    var error = await response.Content.ReadAsStringAsync();

    throw new Exception(
        $"OrderService failed: {(int)response.StatusCode} - {error}"
    );
}

            return await response.Content.ReadFromJsonAsync<OrderDto>();
        }
    }
}