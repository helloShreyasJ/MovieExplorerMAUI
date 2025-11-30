using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer.ViewModels;

public class MovieViewModel : INotifyPropertyChanged
{
    private Movie _selectedMovie;
    private readonly MovieListingService _listingService;

    // Initialize the collection so it's never null
    public ObservableCollection<Movie> Movies { get; } = new ObservableCollection<Movie>();

    public Movie SelectedMovie
    {
        get => _selectedMovie;
        set
        {
            if (_selectedMovie == value)
                return;

            _selectedMovie = value;
            OnPropertyChanged();
        }
    }

    public MovieViewModel(MovieListingService listingService)
    {
        _listingService = listingService;

        // Start loading the movies but don't block the constructor.
        // We intentionally ignore the returned Task here — optionally you can await this
        // from a caller or expose an initialization method.
        _ = LoadMoviesAsync();
    }

    private async Task LoadMoviesAsync()
    {
        try
        {
            var movies = await _listingService.GetMovieListing();

            // Update the ObservableCollection (UI will be notified automatically)
            Movies.Clear();
            foreach (var m in movies)
                Movies.Add(m);

            Debug.WriteLine($"Loaded {Movies.Count} movies into ViewModel.");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading movies: {ex}");
            // optionally surface an error property for the UI here
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}