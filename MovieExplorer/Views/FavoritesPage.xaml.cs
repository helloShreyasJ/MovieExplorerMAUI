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
        // Make sure the file exists and download if needed
        await _movieListingService.EnsureMovieFileExistsAsync();

        // Get the raw json
        string json = File.ReadAllText(
            Path.Combine(FileSystem.AppDataDirectory, "moviesemoji.json")
        );

        // Display JSON in your label
        JsonLbl.Text = json;
    }
}