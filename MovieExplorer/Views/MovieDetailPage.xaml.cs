using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer.Views;

[QueryProperty(nameof(Movie), "Movie")]
public partial class MovieDetailPage : ContentPage
{
    private readonly FavoritesService _favoritesService;
    private Movie _movie;
    public Movie Movie
    {
        get { return _movie;}
        set
        {
            _movie = value;
            OnPropertyChanged();
        }
    }

    public MovieDetailPage(FavoritesService favoritesService)
    {
        InitializeComponent();
        _favoritesService = favoritesService;
        BindingContext = this;
    }
    
    private void LoadMovieDetails()
    {
        if (Movie != null)
        {
            TitleLabel.Text = $"{Movie.title} ({Movie.year})";
            GenreLabel.Text = $"Genre: {Movie.GenreText} {Movie.emoji}";
            RatingLabel.Text = $"Rating: {Movie.rating.ToString()}";
            DirectorLabel.Text = $"Director: {Movie.director}";
            // Poster.Source = Movie.posterUrl;
            PopularityLabel.Text =  $"Popularity: {Movie.popularity}";
            OriginalLangLabel.Text = $"Original Language: {Movie.originalLanguage}";
            OverviewLabel.Text = $"Overview: {Movie.overview}";
        }
    }

    private async void BackButton_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadMovieDetails();
        
        AddToFavorites.ImageSource = _favoritesService.IsFavorite(Movie) ? "heart_off.png" : "heart.png";
    }

    private void AddToFavorites_OnClicked(object? sender, EventArgs e)
    {
        if (_favoritesService.IsFavorite(Movie))
        {
            _favoritesService.RemoveMovie(Movie);
            AddToFavorites.ImageSource = "heart.png";
            DisplayAlert("MovieExplorer", "Removed from Favorites", "OK");
        }
        else
        {
            _favoritesService.AddMovie(Movie);
            AddToFavorites.ImageSource = "heart_off.png";
            DisplayAlert("MovieExplorer", "Added to Favorites", "OK");
        }
    }
}