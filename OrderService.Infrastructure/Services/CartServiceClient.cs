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

        private void ApplyAuthHeader()
        {
            var token = _httpContextAccessor.HttpContext?
                .Request.Headers["Authorization"]
                .ToString();
            if (!string.IsNullOrEmpty(token))
            {
                _http.DefaultRequestHeaders.Authorization =
                    AuthenticationHeaderValue.Parse(token);
            }
        }

        public async Task<CartDto> GetCart()
        {
            ApplyAuthHeader();
            var response = await _http.GetAsync("http://cartservice:8080/api/cart");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CartDto>();
        }

        public async Task<List<CartItemDto>> GetSelectedCart()
        {
            ApplyAuthHeader();
            var response = await _http.GetAsync("http://cartservice:8080/api/cart/selected");
            response.EnsureSuccessStatusCode();
            var items = await response.Content.ReadFromJsonAsync<List<CartItemDto>>();
            return items ?? new List<CartItemDto>();
        }

        public async Task ClearCart()
        {
            ApplyAuthHeader();
            var response = await _http.DeleteAsync("http://cartservice:8080/api/cart/clear");
            response.EnsureSuccessStatusCode();
        }

        public async Task ClearSelectedCart()
        {
            ApplyAuthHeader();
            var response = await _http.PostAsync("http://cartservice:8080/api/cart/selected/clear", null);
            if (!response.IsSuccessStatusCode)
            {
                // Best-effort: selected-clear may not exist; ignore failure.
            }
        }
    }
}
