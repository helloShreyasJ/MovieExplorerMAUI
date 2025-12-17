using System.Collections.ObjectModel;
using System.Text.Json;
using MovieExplorer.Models;

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
        if (movie == null)
        {
            return;
        }

        //
        // if (!Favorites.Contains(movie))
        // {
        //     Favorites.Add(movie);
        //     SaveFavorites();
        // }

        // works
        foreach (var favorite in Favorites)
        {
            if (favorite.title == movie.title
                && favorite.year == movie.year)
            {
                return;
            }
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

        if (Favorites.Contains(movie))
        {
            Favorites.Remove(movie);
            SaveFavorites();
        }
    }

    public bool IsFavorite(Movie movie)
    {
        return Favorites.Contains(movie);
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
}