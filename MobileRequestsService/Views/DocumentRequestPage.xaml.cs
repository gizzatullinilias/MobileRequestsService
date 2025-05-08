using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class DocumentRequestPage : ContentPage
{
	public DocumentRequestPage(DocumentRequestViewModel documentRequestViewModel)
	{
		InitializeComponent();
		BindingContext = documentRequestViewModel;
	}
}