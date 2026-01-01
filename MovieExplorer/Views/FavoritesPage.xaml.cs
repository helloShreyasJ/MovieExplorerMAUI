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
        string result = await DisplayPromptAsync("MovieExplorer", "Type 'YES' to clear all favorites:");
        bool confirm = result?.Equals("YES") ?? false;

        if (!confirm)
        {
            await ClearButton.TranslateTo(20, 0, 200, Easing.CubicIn);
            await ClearButton.TranslateTo(-20, 0, 200, Easing.CubicIn);
            await ClearButton.TranslateTo(20, 0, 200, Easing.CubicIn);
            await ClearButton.TranslateTo(-20, 0, 200, Easing.CubicIn);
            return;
        }

        await ClearButton.TranslateTo(0, -50, 200, Easing.CubicInOut);
        await ClearButton.ScaleTo(1.2, 100, Easing.CubicInOut);
        await ClearButton.ScaleTo(1, 100, Easing.CubicInOut);
        await ClearButton.TranslateTo(0, 0, 200, Easing.CubicInOut);

        if (confirm)
        {
            _favoritesService.ClearFavorites();
        }
    }
}