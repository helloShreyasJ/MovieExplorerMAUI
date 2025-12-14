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
    }

    private void AddToFavorites_OnClicked(object? sender, EventArgs e)
    {
        _favoritesService.AddMovie(Movie);
    }
}