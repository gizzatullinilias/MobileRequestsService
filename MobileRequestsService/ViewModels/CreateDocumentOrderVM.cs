using System.Text.Json;
using ColledgeDocument.Shared.Requests;
using ColledgeDocument.Shared.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileRequestsService.Services;
using MobileRequestsService.ViewModels.Base;

namespace MobileRequestsService.ViewModels;

public partial class CreateDocumentOrderVM : BaseVM
{
    private readonly IDocumentOrderService _documentService;
    private readonly IAccountService _accountService;
    private readonly IDocumentTypeService _documentTypeService;
    private readonly IDepartmentService _departmentService;

    protected override async Task AppearingView()
    {
        try
        {
            IsLoading = true;

            await LoadUserData();
            await LoadDocumentTypesDataAsync();
            await LoadDepartmentsDataAsync();
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

    private async Task LoadUserData()
    {
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

    private async Task LoadDocumentTypesDataAsync()
    {
        var response = await _documentTypeService.GetAllAsync();
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = responseContent;
            return;
        }

        var documentTypes = JsonSerializer.Deserialize<List<DocumentTypeResponse>>(responseContent);
        DocumentTypes = documentTypes!;
    }
    
    private async Task LoadDepartmentsDataAsync()
    {
        var response = await _departmentService.GetAllAsync();
        var responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            ErrorMessage = responseContent;
            return;
        }

        var departments = JsonSerializer.Deserialize<List<DepartmentResponse>>(responseContent);
        Departments = departments!;
    }

    public CreateDocumentOrderVM(
        IDocumentOrderService documentOrderService,
        IAccountService accountService,
        IDocumentTypeService documentTypeService,
        IDepartmentService departmentService)
    {
        _documentService = documentOrderService;
        _accountService = accountService;
        _documentTypeService = documentTypeService;
        _departmentService = departmentService;
    }

    [ObservableProperty]
    private UserResponse? _userResponse;

    [ObservableProperty]
    private string? _errorMessage;

    [ObservableProperty]
    private List<DocumentTypeResponse> _documentTypes = [];
    [ObservableProperty]
    private DocumentTypeResponse? _selectedDocumentType;
    partial void OnSelectedDocumentTypeChanged(DocumentTypeResponse? value)
    {
        if (value == null) return;
        _createDocumentOrderRequest.DocumentTypeId = value.Id;
    }
 
    [ObservableProperty]
    private List<DepartmentResponse> _departments = [];

    [ObservableProperty]
    private DepartmentResponse? _selectedDepartment;
    partial void OnSelectedDepartmentChanged(DepartmentResponse? value)
    {
        if (value == null) return;
        _createDocumentOrderRequest.DepartamnetId = value.Id;
    }

    [ObservableProperty]
    private int _quantity = 1;
    partial void OnQuantityChanged(int value) => _createDocumentOrderRequest.Quantity = value;

    [ObservableProperty]
    private CreateDocumentOrderRequest _createDocumentOrderRequest = new();

    [RelayCommand]
    private async Task CreateDocumentOrderAsync()
    {
        try
        {
            IsLoading = true;

            var response = await _documentService.CreateDocumentOrderAsync(CreateDocumentOrderRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                ErrorMessage = responseContent;
                return;
            }

            await AppShell.Current.GoToAsync("..", true);
        }
        catch (Exception ex)
        {
            ErrorMessage = "Ошибка отправки заявки";
            Console.WriteLine($"Error: {ex}");
        }
        finally
        {
            IsLoading = false;
        }
    }

}