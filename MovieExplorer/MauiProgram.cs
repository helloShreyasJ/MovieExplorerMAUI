using MovieExplorer.Services;
using MovieExplorer.ViewModels;
using MovieExplorer.Views;

namespace MovieExplorer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        // using roboto as the app font
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Roboto-Regular.ttf", "RobotoRegular");
                fonts.AddFont("Roboto-Bold.ttf", "RobotoBold");
            });
        
        // add services and viewmodels as singletons to persist ui state when navigating
        builder.Services.AddLogging();
        builder.Services.AddSingleton<MovieListingService>();
        builder.Services.AddSingleton<FavoritesService>();
        builder.Services.AddSingleton<FavoritesPage>();
        builder.Services.AddSingleton<MovieDetailPage>();
        builder.Services.AddSingleton<MovieViewModel>();
        builder.Services.AddSingleton<HomePage>();
        return builder.Build();
    }
}