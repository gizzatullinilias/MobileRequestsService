using MobileRequestsService.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MobileRequestsService.Services;

namespace MobileRequestsService.ViewModels
{
    public class DocumentHistoryViewModel : INotifyPropertyChanged
    {
        private readonly DocumentService _documentService;
        private DocumentOrderHistoryResponse? _historyResponse;
        private int _currentPage = 1;
        private const int _pageSize = 5;
        private bool _isLoading;
        private string? _errorMessage;

        public DocumentHistoryViewModel(DocumentService documentService)
        {
            _documentService = documentService;
            LoadHistoryCommand = new Command(async () => await LoadHistoryAsync());
            NextPageCommand = new Command(async () => await ChangePage(1), () => CanNavigateNext);
            PrevPageCommand = new Command(async () => await ChangePage(-1), () => CanNavigatePrev);
        }

        public ObservableCollection<DocumentOrderItem> Orders { get; } = new();

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                ((Command)NextPageCommand).ChangeCanExecute();
                ((Command)PrevPageCommand).ChangeCanExecute();
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

        public bool CanNavigateNext => _historyResponse?.IsHaveNextPage ?? false;
        public bool CanNavigatePrev => _historyResponse?.IsHavePrevPage ?? false;

        public ICommand LoadHistoryCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PrevPageCommand { get; }

        public async Task LoadHistoryAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                _historyResponse = await _documentService.GetDocumentOrdersHistoryAsync(_currentPage, _pageSize);

                if (_historyResponse == null)
                {
                    ErrorMessage = "Не удалось загрузить историю заявок";
                    return;
                }

                Orders.Clear();
                foreach (var order in _historyResponse.Data)
                {
                    Orders.Add(order);
                }

                OnPropertyChanged(nameof(CanNavigateNext));
                OnPropertyChanged(nameof(CanNavigatePrev));
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка загрузки истории";
                Console.WriteLine($"Error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task ChangePage(int delta)
        {
            _currentPage += delta;
            await LoadHistoryAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
