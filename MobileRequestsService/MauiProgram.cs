using Microsoft.Extensions.Logging;
using MobileRequestsService.Services;
using MobileRequestsService.ViewModels;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using MobileRequestsService.Views;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MobileRequestsService.Handlers;
using MobileRequestsService.Constants;
using CommunityToolkit.Maui;

namespace MobileRequestsService;

public static class MauiProgram
{

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services
            .AddTransient<AuthorizationHandler>()
            .AddHttpClient(ApplicationConstants.Network.ClientName)
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(ApplicationConstants.Network.BaseAddress))
            .AddHttpMessageHandler<AuthorizationHandler>();

        builder.Services
            .AddTransient<ITokenService, TokenService>()
            .AddTransient<IAccountService, AccountService>()
            .AddTransient<IDocumentOrderService, DocumentOrderService>()
            .AddTransient<IDepartmentService, DepartmentService>()
            .AddTransient<IDocumentTypeService, DocumentTypeService>();

        builder.Services
            .AddTransient<LoginView>()
            .AddTransient<ProfileView>()
            .AddTransient<CreateDocumentOrderView>()
            .AddTransient<DocumentOrdersView>();

        builder.Services
            .AddTransient<DocumentOrdersVM>()
            .AddTransient<CreateDocumentOrderVM>()
            .AddTransient<LoginVM>()
            .AddTransient<ProfileVM>();



        return builder.Build();
    }
}
