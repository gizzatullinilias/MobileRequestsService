using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using MobileRequestsService.Services;
using MobileRequestsService.Models;
using System.Threading.Tasks;

namespace MobileRequestsService.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string? _username;
        private string? _password;
        //private bool _isBusy;
        private string? _errorMessage;
        private readonly AuthenticationService _authService;

        public LoginViewModel(AuthenticationService authService)
        {
            _authService = authService;
            LoginCommand = new Command(async () => await OnLoginClicked());
        }

        public string? Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
                ErrorMessage = string.Empty;
            }
        }

        public string? Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ErrorMessage = string.Empty;
            }
        }

        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public Command LoginCommand { get; }

        private async Task OnLoginClicked()
        {
            var loginRequest = new LoginRequest
            {
                Username = Username,
                Password = Password
            };

            try
            {
                var authResponse = await _authService.LoginAsync(loginRequest);

                if (authResponse != null && !string.IsNullOrEmpty(authResponse.AccessToken))
                {
                    await Shell.Current.GoToAsync("//ProfilePage");
                }
                else 
                {
                    ErrorMessage = "Неверное имя пользователя или пароль.";
                    await App.Current.MainPage.DisplayAlert("Login Failed", ErrorMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла непредвиденная ошибка.";
                Console.WriteLine($"Login error: {ex}");
                await App.Current.MainPage.DisplayAlert("Login Error", ErrorMessage, "OK");
            }

        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}