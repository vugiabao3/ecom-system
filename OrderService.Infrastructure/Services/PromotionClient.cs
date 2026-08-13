using OrderService.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;

namespace OrderService.Infrastructure.Services
{
    public class PromotionClient : IPromotionClient
    {
        private readonly HttpClient _http;
        private readonly ServiceTokenGenerator _token;

        public PromotionClient(HttpClient http, ServiceTokenGenerator token)
        {
            _http = http;
            _token = token;
        }

        public async Task<ApplyPromotionResponse> Apply(string code, decimal total)
        {
            var jwt = _token.Generate();
            // 🔥 DEBUG Ở ĐÂY
            Console.WriteLine("🔥 INTERNAL TOKEN: " + jwt);

            using var request = new HttpRequestMessage(
                HttpMethod.Post,
                "http://promotionservice:8080/api/Promotion/apply"
            );

            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", jwt);

            request.Content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    couponCode = code,
                    totalAmount = total
                }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _http.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<ApplyPromotionResponse>(json)!;
        }
    }
}