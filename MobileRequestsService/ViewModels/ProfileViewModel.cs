using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Maui.Controls;
using MobileRequestsService.Services;
using MobileRequestsService.Models;
using System.Threading.Tasks;

namespace MobileRequestsService.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private UserData? _userData;
        private bool _isBusy;
        private string? _errorMessage;
        private readonly UserDataService _userDataService;

        public ProfileViewModel(UserDataService userDataService)
        {
            _userDataService = userDataService;
            LoadUserDataAsync();
        }

        public UserData? UserData
        {
            get => _userData;
            set
            {
                _userData = value;
                OnPropertyChanged();
            }
        }

        //public bool IsBusy
        //{
        //    get => _isBusy;
        //    set
        //    {
        //        _isBusy = value;
        //        OnPropertyChanged();
        //    }
        //}

        public string? ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged();
            }
        }

        public Command LogoutCommand { get; }

        private async void OnLogout()
        {
            SecureStorage.Default.RemoveAll();
            //await SecureStorage.Default.RemoveAsync("access_token");
            //await SecureStorage.Default.RemoveAsync("refresh_token");
            await Shell.Current.GoToAsync("//LoginPage");
        }
        private async Task LoadUserDataAsync()
        {
            //if (IsBusy) return;
            //IsBusy = true;
            //ErrorMessage = string.Empty;

            try
            {
                UserData = await _userDataService.GetUserDataAsync();
                if (UserData == null)
                {
                    ErrorMessage = "Не удалось загрузить данные пользователя. Пожалуйста, попробуйте войти в систему еще раз.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки пользовательских данных: {ex.Message}");
                ErrorMessage = "При загрузке пользовательских данных произошла непредвиденная ошибка.";
            }
            //finally
            //{
            //    IsBusy = false;
            //}
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}