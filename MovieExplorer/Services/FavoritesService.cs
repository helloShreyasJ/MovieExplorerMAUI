using System.Collections.ObjectModel;
using MovieExplorer.Models;

namespace MovieExplorer.Services;

public class FavoritesService
{
    public ObservableCollection<Movie> Favorites { get; } = new ObservableCollection<Movie>();

    public void AddMovie(Movie movie)
    {
        if (movie == null)
        {
            return;
        }

        if (!Favorites.Contains(movie))
        {
            Favorites.Add(movie);
        }
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
        }
    }

    public bool IsFavorite(Movie movie)
    {
        return Favorites.Contains(movie);
    }
}