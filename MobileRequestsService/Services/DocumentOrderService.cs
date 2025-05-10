using System.Net.Http.Json;
using ColledgeDocument.Shared.Requests;
using MobileRequestsService.Constants;

namespace MobileRequestsService.Services;

public interface IDocumentOrderService
{
    Task<HttpResponseMessage> GetPaginatedAsync(
        int pageNumber, int pageSize);

    Task<HttpResponseMessage> CreateDocumentOrderAsync(
        CreateDocumentOrderRequest request);
}

public class DocumentOrderService : IDocumentOrderService
{
    private readonly IHttpClientFactory _httpClientFactory;
   
    public DocumentOrderService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<HttpResponseMessage> GetPaginatedAsync(
        int pageNumber, int pageSize)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.Network.ClientName);
        return httpClient.GetAsync($"/api/documentorder?pageNumber={pageNumber}&pageSize={pageSize}");
    }

    public Task<HttpResponseMessage> CreateDocumentOrderAsync(CreateDocumentOrderRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.Network.ClientName);
        return httpClient.PostAsJsonAsync("/api/documentorder?", request);
    }
}