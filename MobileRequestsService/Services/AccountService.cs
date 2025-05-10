using MobileRequestsService.Constants;

namespace MobileRequestsService.Services;

public interface IAccountService
{
    Task<HttpResponseMessage> GetMeAsync();
}

public class AccountService : IAccountService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public AccountService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<HttpResponseMessage> GetMeAsync()
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.Network.ClientName);
        return httpClient.GetAsync("/api/account/me");
    }
}