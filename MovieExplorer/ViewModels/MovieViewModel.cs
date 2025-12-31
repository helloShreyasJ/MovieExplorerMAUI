using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MovieExplorer.Models;
using MovieExplorer.Services;

namespace MovieExplorer.ViewModels;

public class MovieViewModel : INotifyPropertyChanged
{
    private double _loadProgress;
    private bool _isLoading;
    private readonly MovieListingService _listingService; // loads movies from donnys github
    private Movie _selectedMovie;
    private List<Movie> _allMovies = new List<Movie>(); // not bound to UI. stores ALL movies
    public ObservableCollection<Movie> Movies { get; } = new ObservableCollection<Movie>(); // bound to UI. shows filtered movies
    private string _searchText = string.Empty;

    public double LoadProgress
    {
        get { return _loadProgress; }
        set
        {
            _loadProgress = value;
            OnPropertyChanged();
        }
    }
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
            ApplySearchFilter();
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

    public async Task LoadMoviesAsync()
    {
        try
        {
            // _isLoading = true; this wasn't working before.. why? because OnPropertyChanged never gets called so no updates
            IsLoading = true; // should work now
            Movies.Clear(); // removes existing movies when function gets called. fixes ui freeze
            LoadProgress = 0; // reset progress bar
            
            var progress = new Progress<double>(p => LoadProgress = p); // progress object
            
            // get movies from service
            var movies = await _listingService.GetMovieListing(progress);
            _allMovies = movies.ToList();
            ApplySearchFilter(); // if search text contains something match immediately
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading movies: {ex}");
        }
        finally
        {
            LoadProgress = 1;
            IsLoading = false;
        }
    }

    public void ApplySearchFilter()
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
    
    // sorts/orders movie using the typa filter passed through parameters
    public void ApplySort(string filterType)
    {
        IEnumerable<Movie> sorted;

        switch (filterType)
        {
            case "Title":
                sorted = _allMovies.OrderBy(m => m.title);
                break;
            case "Year":
                sorted = _allMovies.OrderByDescending(m => m.year);
                break;
            case "Genre":
                sorted = _allMovies.OrderBy(m => m.GenreText);
                break;
            case "Rating":
                sorted = _allMovies.OrderByDescending(m => m.rating);
                break;
            case "Popularity":
                sorted = _allMovies.OrderByDescending(m => m.popularity);
                break;
            default:
                sorted = _allMovies;
                break;
        };

        Movies.Clear();
        foreach (var movie in sorted)
            Movies.Add(movie);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}