using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class LoginView : ContentPage
{
	public LoginView(LoginVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}