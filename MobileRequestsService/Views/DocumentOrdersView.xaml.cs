using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class DocumentOrdersView : ContentPage
{
	public DocumentOrdersView(DocumentOrdersVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}