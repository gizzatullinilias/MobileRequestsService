using MobileRequestsService.Models;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;
using MobileRequestsService.Models;
using System.Net.Http.Headers;

namespace MobileRequestsService.Services
{
    public class DocumentService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl = "http://10.0.2.2:9090/api";
        private readonly AuthenticationService _authService;

        public DocumentService(HttpClient httpClient, AuthenticationService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        public async Task<List<DocumentType>?> GetDocumentTypesAsync()
        {
            try
            {
                await _authService.AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/documenttype");
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<List<DocumentType>>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Department>?> GetDepartmentsAsync()
        {
            try
            {
                await _authService.AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/department");
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<List<Department>>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> SubmitDocumentOrderAsync(DocumentOrderRequest request)
        {
            try
            {
                await _authService.AddAuthorizationHeader();
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/documentorder", content);
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<DocumentOrderHistoryResponse?> GetDocumentOrdersHistoryAsync(int page = 1, int pageSize = 5)
        {
            try
            {
                await _authService.AddAuthorizationHeader();
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/documentorder/me?page={page}&pageSize={pageSize}");
                response.EnsureSuccessStatusCode();
                return JsonSerializer.Deserialize<DocumentOrderHistoryResponse>(await response.Content.ReadAsStringAsync());
            }
            catch
            {
                return null;
            }
        }
    }
}