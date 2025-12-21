using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer.ViewModels;

public class MovieViewModel : INotifyPropertyChanged
{
    private bool _isLoading;
    private readonly MovieListingService _listingService; // loads movies from donnys github
    private Movie _selectedMovie;
    private List<Movie> _allMovies = new List<Movie>(); // not bound to UI. stores ALL movies
    public ObservableCollection<Movie> Movies { get; } = new ObservableCollection<Movie>(); // bound to UI. shows filtered movies
    private string _searchText = string.Empty;

    public bool IsLoading
    {
        get { return _isLoading; }
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }
    public string SearchText
    {
        get { return _searchText; }
        set
        {
            if (_searchText == value)
            {
                return;
            }
            _searchText = value;
            OnPropertyChanged();
            ApplyFilter();
        }
    }
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
        _ = LoadMoviesAsync(); // start loading movies
    }

    private async Task LoadMoviesAsync()
    {
        try
        {
            // _isLoading = true; this wasn't working before.. why? because OnPropertyChanged never gets called so no updates
            IsLoading = true; // should work now
            // get movies from service
            var movies = await _listingService.GetMovieListing();
            _allMovies = movies.ToList();
            ApplyFilter(); // if search text contains something match immediately
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading movies: {ex}");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public void ApplyFilter()
    {
        Movies.Clear();

        if (string.IsNullOrWhiteSpace(SearchText))
        {
            foreach (Movie movie in _allMovies)
            {
                Movies.Add(movie);
            }

            return;
        }

        foreach (Movie movie in _allMovies)
        {
            bool titleMatches =
                movie.title.Contains(SearchText, StringComparison.OrdinalIgnoreCase);

            if (titleMatches)
            {
                Movies.Add(movie);
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}