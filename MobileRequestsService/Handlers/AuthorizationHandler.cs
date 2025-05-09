using MobileRequestsService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MobileRequestsService.Handlers
{
    public class AuthorizationHandler : DelegatingHandler
    {
        private readonly AuthenticationService _authService;

        public AuthorizationHandler(AuthenticationService authService)
        {
            _authService = authService;
        }

        private bool IsRefreshing;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var accessToken = await _authService.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(accessToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}