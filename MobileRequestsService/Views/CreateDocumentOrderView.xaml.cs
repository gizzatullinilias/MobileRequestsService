using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class CreateDocumentOrderView : ContentPage
{
	public CreateDocumentOrderView(CreateDocumentOrderVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}