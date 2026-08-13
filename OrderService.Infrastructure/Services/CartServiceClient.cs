using Microsoft.AspNetCore.Http;
using OrderService.Application.DTOs;
using OrderService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace OrderService.Infrastructure.Services
{
    public class CartServiceClient : ICartServiceClient
    {
        private readonly HttpClient _http;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartServiceClient(HttpClient http, IHttpContextAccessor httpContextAccessor)
        {
            _http = http;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CartDto> GetCart()
        {
            var token = _httpContextAccessor.HttpContext?
                .Request.Headers["Authorization"]
                .ToString();

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(token);
            }

            var response = await _http.GetAsync("http://cartservice:8080/api/cart");

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<CartDto>();
        }
        public async Task ClearCart()
        {
            var token = _httpContextAccessor.HttpContext?
                .Request.Headers["Authorization"]
                .ToString();

            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(token);
            }

            var response = await _http.DeleteAsync("http://cartservice:8080/api/cart/clear");

            response.EnsureSuccessStatusCode();
        }
    }
}
