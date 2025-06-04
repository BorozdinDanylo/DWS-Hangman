using CommunityToolkit.Maui;
using Hangmen.BL.Implementation;
using Hangmen.BL.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hangman;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit(options =>
            {
                options.SetShouldEnableSnackbarOnWindows(true);
            })
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        builder.Services.AddSingleton<IWordPool, WordPool>();
        builder.Services.AddSingleton<IGameManager, GameManager>();

        return builder.Build();
    }
}
