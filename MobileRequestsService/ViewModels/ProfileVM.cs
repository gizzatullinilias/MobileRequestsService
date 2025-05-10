using MobileRequestsService.Services;
using MobileRequestsService.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using ColledgeDocument.Shared.Responses;
using CommunityToolkit.Mvvm.Input;
using MobileRequestsService.ViewModels.Base;
using MobileRequestsService.Constants;
using System.Text.Json;


namespace MobileRequestsService.ViewModels
{
    public partial class ProfileVM : BaseVM
    {
        private readonly IAccountService _accountService;

        protected override async Task AppearingView()
        {
            try
            {
                IsLoading = true;

                await LoadUserDataAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки пользовательских данных: {ex.Message}");
                ErrorMessage = "При загрузке пользовательских данных произошла непредвиденная ошибка.";

            }
            finally
            {
                IsLoading = false;
            }
        }

        public ProfileVM(
            IAccountService accountService)
        {
            _accountService = accountService;
        }

        [ObservableProperty]
        private string? _errorMessage;


        [ObservableProperty]
        private UserResponse _userResponse;


        [RelayCommand]
        public async Task LogOutAsync()
        {
            SecureStorage.Remove(ApplicationConstants.Security.AccessToken);
            SecureStorage.Remove(ApplicationConstants.Security.RefreshToken);

            await AppShell.Current.GoToAsync("//" + nameof(LoginView), true);
        }

        [RelayCommand]
        private async Task GoToCreateDocumentOrderViewAsync() =>  
            await AppShell.Current.GoToAsync("//" + nameof(CreateDocumentOrderView), true);
       

        [RelayCommand]
        private Task GoToDocumentOrdersViewAsync() =>
             AppShell.Current.GoToAsync("//" + nameof(DocumentOrdersView), true);


        [RelayCommand]
        public async Task LoadUserDataAsync()
        {
            try
            {
                IsLoading = true;

                ErrorMessage = string.Empty;

                var response = await _accountService.GetMeAsync();
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = responseContent;
                    return;
                }
                
                var userResponse = JsonSerializer.Deserialize<UserResponse>(responseContent);
                UserResponse = userResponse!;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки пользовательских данных: {ex.Message}");
                ErrorMessage = "При загрузке пользовательских данных произошла непредвиденная ошибка.";
            }
            finally
            {
                IsLoading = false;
            }

        }

    }
}