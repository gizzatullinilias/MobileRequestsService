using MobileRequestsService.Models;
using MobileRequestsService.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MobileRequestsService.ViewModels
{
    public class DocumentRequestViewModel : INotifyPropertyChanged
    {
        private readonly DocumentService _documentService;
        private readonly UserDataService _userDataService;
        private UserData? _userData;
        private string? _errorMessage;
        private bool _isLoading;
        private DocumentType? _selectedDocumentType;
        private Department? _selectedDepartment;
        private int _quantity = 1;

        public DocumentRequestViewModel(DocumentService documentService, UserDataService userDataService)
        {
            _documentService = documentService;
            _userDataService = userDataService;
            LoadDataCommand = new Command(async () => await LoadInitialDataAsync());
            SubmitCommand = new Command(async () => await SubmitRequestAsync());
        }

        public ObservableCollection<DocumentType> DocumentTypes { get; } = new();
        public ObservableCollection<Department> Departments { get; } = new();

        public UserData? UserData
        {
            get => _userData;
            set
            {
                _userData = value;
                OnPropertyChanged();
            }
        }

        public DocumentType? SelectedDocumentType
        {
            get => _selectedDocumentType;
            set
            {
                _selectedDocumentType = value;
                OnPropertyChanged();
            }
        }

        public Department? SelectedDepartment
        {
            get => _selectedDepartment;
            set
            {
                _selectedDepartment = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand SubmitCommand { get; }

        public async Task LoadInitialDataAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = null;

                UserData = await _userDataService.GetUserDataAsync();
                var documentTypesTask = _documentService.GetDocumentTypesAsync();
                var departmentsTask = _documentService.GetDepartmentsAsync();

                await Task.WhenAll(documentTypesTask, departmentsTask);

                DocumentTypes.Clear();
                if (documentTypesTask.Result != null)
                {
                    foreach (var type in documentTypesTask.Result)
                    {
                        DocumentTypes.Add(type);
                    }
                }

                Departments.Clear();
                if (departmentsTask.Result != null)
                {
                    foreach (var dept in departmentsTask.Result)
                    {
                        Departments.Add(dept);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Ошибка загрузки данных";
                Console.WriteLine($"Error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task SubmitRequestAsync()
        {
            if (SelectedDocumentType == null || SelectedDepartment == null)
            {
                ErrorMessage = "Выберите тип документа и отдел";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = null;

                var request = new DocumentOrderRequest
                {
                    DocumentTypeId = SelectedDocumentType.Id,
                    DepartmentId = SelectedDepartment.Id,
                    Quantity = Quantity
                };

                var success = await _documentService.SubmitDocumentOrderAsync(request);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Успешно", "Заявление подано", "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    ErrorMessage = "Ошибка при подаче заявления";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Произошла ошибка";
                Console.WriteLine($"Error: {ex}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}