using MobileRequestsService.ViewModels;

namespace MobileRequestsService.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel profileView)
	{
		InitializeComponent();
		BindingContext = profileView;
	}
}