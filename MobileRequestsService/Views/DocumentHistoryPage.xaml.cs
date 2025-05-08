using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class DocumentHistoryPage : ContentPage
{
	public DocumentHistoryPage(DocumentHistoryViewModel documentHistoryViewModel)
	{
		InitializeComponent();
		BindingContext = documentHistoryViewModel;
	}
}