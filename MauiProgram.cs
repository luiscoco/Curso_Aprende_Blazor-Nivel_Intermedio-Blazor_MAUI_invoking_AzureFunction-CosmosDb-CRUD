using Microsoft.Extensions.Logging;
using MAUIBlazorAzureFunctionCRUDCosmosDB.Services;

namespace MAUIBlazorAzureFunctionCRUDCosmosDB
{
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://mycosmosdbcrudazurefunction.azurewebsites.net/") });
            builder.Services.AddScoped<CosmosDbService>();

            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
