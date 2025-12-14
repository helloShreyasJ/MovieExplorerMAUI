using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer.Views;

public partial class FavoritesPage : ContentPage
{
    private readonly MovieListingService _movieListingService;
    
    public FavoritesPage(FavoritesService favoritesService)
    {
        InitializeComponent();
        BindingContext = favoritesService;
    }
}