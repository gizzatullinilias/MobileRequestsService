using MobileRequestsService.Constants;

namespace MobileRequestsService.Services;

public interface IDocumentTypeService
{
    Task<HttpResponseMessage> GetAllAsync();

}
public class DocumentTypeService : IDocumentTypeService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DocumentTypeService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<HttpResponseMessage> GetAllAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.Network.ClientName);
        return httpClient.GetAsync("/api/documenttype");
    }
}
