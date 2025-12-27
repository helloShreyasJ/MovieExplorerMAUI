using MovieExplorer.Models;
using MovieExplorer.ViewModels;

namespace MovieExplorer.Views;

public partial class HomePage : ContentPage
{
    public HomePage(MovieViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // getting and setting properties from vm
    }

    private async void CollectionView_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is Movie selectedMovie) {
            var parameters = new Dictionary<string, object>
            {
                { "Movie", selectedMovie }
            };

        await Shell.Current.GoToAsync(nameof(MovieDetailPage), parameters);
        ((CollectionView)sender).SelectedItem = null;
        }
    }

    private async void SettingsButton_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(Settings));
    }

    private async void SortButton_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(FilterPage));
    }

    private async void ReloadButton_OnClicked(object? sender, EventArgs e)
    {
        if (BindingContext is MovieViewModel vm)
        {
            await vm.LoadMoviesAsync();
        }
    }
}