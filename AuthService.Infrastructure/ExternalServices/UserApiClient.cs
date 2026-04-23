using EcomSystem.Contracts.Users;
using AuthService.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;



namespace AuthService.Infrastructure.ExternalServices
{
    public class UserApiClient : IUserServiceClient
    {
        private readonly HttpClient _httpClient;

        public UserApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string email)
        {
            var res = await _httpClient.GetAsync($"/users/by-email?email={email}");

            if (res.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (!res.IsSuccessStatusCode)
                throw new Exception(await res.Content.ReadAsStringAsync());

            var content = await res.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(content))
                return null;

            return JsonSerializer.Deserialize<UserDto>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        public async Task<UserDto?> GetUserByIdAsync(Guid userId)
        {
            var res = await _httpClient.GetAsync($"/users/{userId}");

            if (res.StatusCode == HttpStatusCode.NotFound)
                return null;

            if (!res.IsSuccessStatusCode)
                throw new Exception(await res.Content.ReadAsStringAsync());

            return await res.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<Guid> CreateUserAsync(CreateUserRequest request)
        {
            var res = await _httpClient.PostAsJsonAsync("/users/create", request);

            if (!res.IsSuccessStatusCode)
                throw new Exception(await res.Content.ReadAsStringAsync());

            var user = await res.Content.ReadFromJsonAsync<UserDto>();

            return user!.Id;
        }

        public async Task UpdatePasswordAsync(Guid userId, string newPasswordHash)
        {
            var request = new UpdateUserRequest
            {
                PasswordHash = newPasswordHash
            };

            var res = await _httpClient.PutAsJsonAsync($"/users/{userId}", request);

            if (!res.IsSuccessStatusCode)
                throw new Exception(await res.Content.ReadAsStringAsync());
        }

        public async Task LogoutAllDevicesAsync(Guid userId)
        {
            var res = await _httpClient.PostAsync($"/users/{userId}/logout-all", null);

            if (!res.IsSuccessStatusCode)
                throw new Exception(await res.Content.ReadAsStringAsync());
        }
    }
}
