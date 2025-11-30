using MovieExplorer.Services;

namespace MovieExplorer.Views;

public partial class FavoritesPage : ContentPage
{
    private readonly MovieListingService _movieListingService;
    
    public FavoritesPage(MovieListingService movieListingService)
    {
        InitializeComponent();
        _movieListingService = movieListingService;
        
        LoadJsonAsync();
    }

    private async void LoadJsonAsync()
    {
        await _movieListingService.EnsureMovieFileExistsAsync();
        string json = File.ReadAllText(
            Path.Combine(FileSystem.AppDataDirectory, "moviesemoji.json")
        );
    }
}