using MobileRequestsService.Models;
using System.Text;
using System.Text.Json;
using Microsoft.Maui.Storage;
using System.Net.Http.Headers;
using MobileRequestsService.Handlers;
using System.Net.Http;

namespace MobileRequestsService.Services
{
    public class DocumentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
       
        public DocumentService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<DocumentType>?> GetDocumentTypesAsync()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync("/documenttype");
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
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync("/department");
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
                //await _authService.AddAuthorizationHeader();
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.PostAsync("/documentorder", content);
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
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync($"/documentorder/me?page={page}&pageSize={pageSize}");
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