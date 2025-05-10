using MobileRequestsService.Constants;

namespace MobileRequestsService.Services;

public interface IDepartmentService
{
    Task<HttpResponseMessage> GetAllAsync();
}
public class DepartmentService : IDepartmentService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public DepartmentService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<HttpResponseMessage> GetAllAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.Network.ClientName);
        return httpClient.GetAsync("api/department");
    }
}
