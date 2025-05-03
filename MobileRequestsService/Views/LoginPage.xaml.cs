using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel loginView)
	{
		InitializeComponent();
		BindingContext = loginView;
	}
}