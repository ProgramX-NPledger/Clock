using Clock.Maui.Data;
using Microsoft.Extensions.Logging;

namespace Clock.Maui;

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
                fonts.AddFont("Exo-Medium.otf", "ExoMedium");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        string fileName = Path.Combine(FileSystem.AppDataDirectory, "workitems.sqlite.db");
        builder.Services.AddSingleton<WorkItemRepository>(s =>
            ActivatorUtilities.CreateInstance<WorkItemRepository>(s,fileName));
        
        
        return builder.Build();
    }
}