using MovieExplorer.Models;
using MovieExplorer.Services;
using MovieExplorer.ViewModels;

namespace MovieExplorer.Views;

public partial class FavoritesPage : ContentPage
{
    private readonly FavoritesService _favoritesService;
    
    public FavoritesPage(FavoritesService favoritesService)
    {
        InitializeComponent();
        _favoritesService = favoritesService;
        BindingContext = favoritesService;
    }

    private async void Clear_OnClicked(object? sender, EventArgs e)
    {
        string result = await DisplayPromptAsync("MovieExplorer", "Type YES to clear all favorites:");
        bool confirm = result?.Equals("YES") ?? false;

        if (confirm)
        {
            _favoritesService.ClearFavorites();
        }
    }
}