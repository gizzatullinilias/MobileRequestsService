using Microsoft.Extensions.Logging;
using MobileRequestsService.Services;
using MobileRequestsService.ViewModels;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using MobileRequestsService.Views;

namespace MobileRequestsService;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif

        builder.Services.AddHttpClient<AuthenticationService>(client =>
        {
            
        });

        builder.Services.AddSingleton<DocumentHistoryViewModel>();
        builder.Services.AddSingleton<DocumentService>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<ProfileViewModel>();
        builder.Services.AddSingleton<AuthenticationService>();
        builder.Services.AddSingleton<UserDataService>();
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<ProfilePage>();
        builder.Services.AddTransient<DocumentRequestViewModel>();
        builder.Services.AddTransient<DocumentHistoryPage>();


        return builder.Build();
	}
}
