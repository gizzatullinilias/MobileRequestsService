using System.Text.Json;
using System.Net.Http.Headers;
using MobileRequestsService.Models;

namespace MobileRequestsService.Services
{
    public class UserDataService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authService;
        private readonly string _apiBaseUrl = "http://localhost:9090/api";

        public UserDataService(HttpClient httpClient, AuthenticationService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task<UserData?> GetUserDataAsync()
        {
            _authService.AddAuthorizationHeader();
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/api/account/me");
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var userData = JsonSerializer.Deserialize<UserData>(responseString);
                return userData;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                if (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    await _authService.ClearTokensAsync();
                }
                return null;
            }
        }
    }
}