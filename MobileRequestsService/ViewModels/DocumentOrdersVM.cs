using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using ColledgeDocument.Shared.Responses;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MobileRequestsService.Services;
using MobileRequestsService.ViewModels.Base;
using MobileRequestsService.Views;

namespace MobileRequestsService.ViewModels
{
    public partial class DocumentOrdersVM : BaseVM
    {
        private readonly IDocumentOrderService _documentService;

        protected override async Task AppearingView()
        {
            try
            {
                IsLoading = true;
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

        public DocumentOrdersVM(
            IDocumentOrderService documentService)
        {
            _documentService = documentService;
        }

        private int pageNumber = 1;
        private int pageSize = 5;

        [ObservableProperty]
        private bool _isHaveNextPage;

        public ObservableCollection<DocumentOrderResponse> DocumentOrders { get; } = [];

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string? _errorMessage;

        [RelayCommand]
        private Task GoToCreateDocumentOrderViewAsync() => AppShell.Current.GoToAsync(nameof(CreateDocumentOrderView), true);

        [RelayCommand]
        public async Task LoadDocumentOrdersAsync()
        {
            try
            {
                ErrorMessage = null;

                var response = await _documentService.GetPaginatedAsync(pageNumber, pageSize);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = responseContent;
                    return;
                }

                var paginatedResponse = JsonSerializer.Deserialize<PaginationResponse<DocumentOrderResponse>>(responseContent);
                
                DocumentOrders.Clear();
                paginatedResponse!.Data.ForEach(DocumentOrders.Add);
                IsHaveNextPage = paginatedResponse.IsHaveNextPage;
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

        [RelayCommand]
        private async Task LoadNextData()
        {
            try
            {
                ErrorMessage = null;

                pageNumber += 1;

                var response = await _documentService.GetPaginatedAsync(pageNumber, pageSize);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    ErrorMessage = responseContent;
                    return;
                }

                var paginatedResponse = JsonSerializer.Deserialize<PaginationResponse<DocumentOrderResponse>>(responseContent);

                paginatedResponse!.Data.ForEach(DocumentOrders.Add);
                IsHaveNextPage = paginatedResponse.IsHaveNextPage;
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
    }
}
