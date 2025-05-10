using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using MobileRequestsService.Services;
using System.Threading.Tasks;
using MobileRequestsService.Views;
using MobileRequestsService.ViewModels.Base;
using CommunityToolkit.Mvvm.ComponentModel;
using ColledgeDocument.Shared.Requests;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;
using ColledgeDocument.Shared.Responses;
using MobileRequestsService.Constants;

namespace MobileRequestsService.ViewModels
{
    public partial class LoginVM : BaseVM
    {
        private readonly ITokenService _tokenService;

        protected override async Task AppearingView()
        {
            try
            {
                IsLoading = true;
                
                var accessToken = await SecureStorage.GetAsync(ApplicationConstants.Security.AccessToken);
                if (string.IsNullOrWhiteSpace(accessToken)) return;

                await AppShell.Current.GoToAsync("//" + nameof(DocumentOrdersView), true);
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла непредвиденная ошибка.";
                Console.WriteLine($"Login error: {ex}");
                await AppShell.Current.DisplayAlert("Login Error", ErrorMessage, "OK");
            }
            finally
            {
                IsLoading = false;
            }

        }

        public LoginVM(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [ObservableProperty]
        private TokenRequest _tokenRequest = new();

        [ObservableProperty]
        public string? _errorMessage;

        [RelayCommand]
        private async Task LoginAsync()
        {
            try
            { 
                IsLoading = true;

                var response = await _tokenService.LoginAsync(TokenRequest);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = responseContent;
                    return;
                }

                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                await SecureStorage.SetAsync(ApplicationConstants.Security.AccessToken, tokenResponse!.AccessToken);
                await SecureStorage.SetAsync(ApplicationConstants.Security.RefreshToken, tokenResponse!.RefreshToken);

                await AppShell.Current.GoToAsync("//" + nameof(DocumentOrdersView));
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла непредвиденная ошибка.";
                Console.WriteLine($"Login error: {ex}");
                await AppShell.Current.DisplayAlert("Login Error", ErrorMessage, "OK");
            }
            finally
            {
                IsLoading = false;
            }

        }
    }
}