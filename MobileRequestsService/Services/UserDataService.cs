using MobileRequestsService.Handlers;
using System.Net.Http.Headers;
using System.Text.Json;
using MobileRequestsService.Models;

namespace MobileRequestsService.Services
{
    public class UserDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UserData?> GetUserDataAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ApiClient");
                var response = await httpClient.GetAsync("account/me");
                response.EnsureSuccessStatusCode();

                using var stream = await response.Content.ReadAsStreamAsync();
                return await JsonSerializer.DeserializeAsync<UserData>(stream);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return null;
            }
        }
    }
}