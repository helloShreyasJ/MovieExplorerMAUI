using System.Collections.ObjectModel;
using System.Text.Json;
using MovieExplorer.Models;
using MovieExplorer.Views;

namespace MovieExplorer.Services;

public class FavoritesService
{
    private const string FavoritesKey = "favorites_movies";
    public ObservableCollection<Movie> Favorites { get; } = new ObservableCollection<Movie>();

    public FavoritesService()
    {
        LoadFavorites();
    }
    public void AddMovie(Movie movie)
    {
        if (movie == null) return;
        
        //stored favorite object
        var existing = Favorites.FirstOrDefault(f => f.title == movie.title && f.year == movie.year);

        if (existing != null)
        {
            Favorites.Remove(existing);
            SaveFavorites();
            return;
        }

        Favorites.Add(movie);
        SaveFavorites();
    }

    public void RemoveMovie(Movie movie)
    {
        if (movie == null)
        {
            return;
        }
        
        //stored favorite object
        var existing = Favorites.FirstOrDefault(f => f.title == movie.title && f.year == movie.year);

        if (existing != null)
        {
            Favorites.Remove(existing); 
            SaveFavorites();
        }
    }

    public bool IsFavorite(Movie movie)
    {
        // return Favorites.Contains(movie);
        return Favorites.Any(f => f.title == movie.title && f.year == movie.year);
    }

    private void SaveFavorites()
    {
        string json = JsonSerializer.Serialize(Favorites);
        Preferences.Set(FavoritesKey, json);
    }

    private void LoadFavorites()
    {
        string json = Preferences.Get(FavoritesKey, "");
        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }
        
        var movies = JsonSerializer.Deserialize<List<Movie>>(json);
        
        Favorites.Clear(); // to avoid duplicates
        foreach (var movie in movies)
        {
            Favorites.Add(movie);
        }
    }

    public void ClearFavorites()
    {
        Favorites.Clear();
        SaveFavorites();
    }
}