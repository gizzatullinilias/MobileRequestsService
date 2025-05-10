using ColledgeDocument.Shared.Requests;
using MobileRequestsService.Constants;
using System.Net.Http.Json;

namespace MobileRequestsService.Services;

public interface ITokenService
{
    Task<HttpResponseMessage> LoginAsync(TokenRequest request);
    Task<HttpResponseMessage> RefreshAsync(RefreshTokenRequest request);
}

public class TokenService : ITokenService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public TokenService(
        IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public Task<HttpResponseMessage> LoginAsync(TokenRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.Network.ClientName);
        return httpClient.PostAsJsonAsync("/api/token", request);
    }

    public Task<HttpResponseMessage> RefreshAsync(RefreshTokenRequest request)
    {
        var httpClient = _httpClientFactory.CreateClient(ApplicationConstants.Network.ClientName);
        return httpClient.PostAsJsonAsync("/api/token/refresh-token", request);      
    }
}