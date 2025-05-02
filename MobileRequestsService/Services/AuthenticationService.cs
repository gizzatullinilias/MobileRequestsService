using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;
using MobileRequestsService.Models;
using System.Net.Http.Headers;
using System.IdentityModel.Tokens.Jwt;

namespace MobileRequestsService.Services
{
    public class AuthenticationService
    {
        private readonly HttpClient _httpClient;
        private const string AccessTokenKey = "access_token";
        private const string RefreshTokenKey = "refresh_token";
        private readonly string _apiBaseUrl = "http://localhost:9090/api";

        public AuthenticationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
         
        public async Task<AuthResponse?> LoginAsync(LoginRequest loginRequest)
        {
            var json = JsonSerializer.Serialize(loginRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/token", content);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseString);

                if (authResponse != null)
                {
                    await SecureStorage.Default.SetAsync(AccessTokenKey, authResponse.AccessToken);
                    await SecureStorage.Default.SetAsync(RefreshTokenKey, authResponse.RefreshToken);
                }

                return authResponse;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Request Error: {ex.Message}");
                return null;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                return null;
            }
        }

        public async Task<string?> GetAccessTokenAsync()
        {
            return await SecureStorage.Default.GetAsync(AccessTokenKey);
        }

        public async Task<bool> RefreshAccessTokenAsync()
        {
            string? refreshToken = await SecureStorage.Default.GetAsync(RefreshTokenKey);
            if (string.IsNullOrEmpty(refreshToken))
            {
                return false;
            }

            var request = new
            {
                refreshToken = refreshToken
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/token/refresh-token", content);
                response.EnsureSuccessStatusCode();
                var responseString = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<AuthResponse>(responseString);

                if (authResponse != null)
                {
                    await SecureStorage.Default.SetAsync(AccessTokenKey, authResponse.AccessToken);
                    await SecureStorage.Default.SetAsync(RefreshTokenKey, authResponse.RefreshToken);
                    return true;
                }
                return false;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Refresh Token Error: {ex.Message}");
                await ClearTokensAsync();
                return false;
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON Deserialization Error: {ex.Message}");
                await ClearTokensAsync();
                return false;
            }
        }

        public async Task<bool> IsTokenExpiredAsync()
        {
            string? token = await GetAccessTokenAsync();
            if (string.IsNullOrEmpty(token))
            {
                return true;
            }

            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                var expiry = jsonToken.ValidTo;
                return expiry < DateTime.UtcNow;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public async Task<bool> ClearTokensAsync()
        {
            try
            {
                await SecureStorage.Default.RemoveAsync(AccessTokenKey);
                await SecureStorage.Default.RemoveAsync(RefreshTokenKey);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void AddAuthorizationHeader()
        {
            if (IsTokenExpiredAsync().Result)
            {
                RefreshAccessTokenAsync().Wait();
            }
            var accessToken = GetAccessTokenAsync().Result;
            if (!string.IsNullOrEmpty(accessToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }
    }
}