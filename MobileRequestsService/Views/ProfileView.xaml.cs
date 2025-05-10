using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class ProfileView : ContentPage
{
	public ProfileView(ProfileVM vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}