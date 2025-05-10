using ColledgeDocument.Shared.Requests;
using ColledgeDocument.Shared.Responses;
using MobileRequestsService.Constants;
using MobileRequestsService.Services;
using MobileRequestsService.Views;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;

namespace MobileRequestsService.Handlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public AuthorizationHandler(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        private bool isRefreshing;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await SecureStorage.GetAsync(ApplicationConstants.Security.AccessToken);
            if (!string.IsNullOrWhiteSpace(accessToken))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await base.SendAsync(request, cancellationToken);
            if (isRefreshing || string.IsNullOrWhiteSpace(accessToken) ||
                response.StatusCode != HttpStatusCode.Unauthorized)
                return response;
   
                try
                {
                    isRefreshing = true;

                    var refreshToken = await SecureStorage.GetAsync(ApplicationConstants.Security.RefreshToken);
                    if (!string.IsNullOrWhiteSpace(refreshToken))
                    {
                        var refreshTokenRequest = new RefreshTokenRequest(accessToken, refreshToken);
                        var refreshTokenResponse = await _tokenService.RefreshAsync(refreshTokenRequest);
                        if (!refreshTokenResponse.IsSuccessStatusCode)
                        {
                            await AppShell.Current.GoToAsync("//" + nameof(LoginView), true);
                            return response;
                        }
                        var reshTokenResponseContent = await refreshTokenResponse.Content.ReadAsStringAsync();
                        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(reshTokenResponseContent);

                        await SecureStorage.SetAsync(ApplicationConstants.Security.AccessToken, tokenResponse!.AccessToken);
                        await SecureStorage.SetAsync(ApplicationConstants.Security.RefreshToken, tokenResponse!.RefreshToken);

                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse!.AccessToken);
                        response = await base.SendAsync(request, cancellationToken);
                    }
                }
                finally
                {
                    isRefreshing = false;
                }
            return response;
        }
    }
}