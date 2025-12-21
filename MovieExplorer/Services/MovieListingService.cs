using System.Text.Json;
using MovieExplorer.Models;
using Path = System.IO.Path;
namespace MovieExplorer.Services;

public class MovieListingService
{
    private const string MovieListUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";
    private readonly string _localFilePath;
    private const string TmdbApiKey = Key.personalKey; // IMPORTANT: tmdb api key needs to be here to get the images
    private const string TmdbSearchUrl = "https://api.themoviedb.org/3/search/movie";
    private readonly HttpClient _client; // using only one HttpClient

    public MovieListingService()
    {
        string localFolder = FileSystem.AppDataDirectory; // local path for getting list
        _localFilePath = Path.Combine(localFolder, "moviesemoji.json");
        _client = new HttpClient();
    }

    private async Task<string> GetPosterUrlAsync(string? title, int year)
    {
        System.Diagnostics.Debug.WriteLine(
            $"TMDB search: {title} ({year})");
        try
        {
            string url =  $"{TmdbSearchUrl}?api_key={TmdbApiKey}&query={Uri.EscapeDataString(title)}";
            // $"{TmdbSearchUrl}?api_key={TmdbApiKey}&query={Uri.EscapeDataString(title)}&year={year}"; test: is adding year as query breaking stuff?
                                                                                                        // no
            string json = await _client.GetStringAsync(url);
            System.Diagnostics.Debug.WriteLine(json); // debug test
            using JsonDocument doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");

            if (results.GetArrayLength() == 0)
            {
                return null;
            }

            var posterPath = results[0].GetProperty("poster_path").GetString();

            if (string.IsNullOrEmpty(posterPath))
            {
                return null;
            }

            return $"https://image.tmdb.org/t/p/w500{posterPath}";
        }
        catch
        {
            return null;
        }
    }

    private async Task DownloadAndSaveListAsync()
    {
        try
        {
            string movieList = await _client.GetStringAsync(MovieListUrl);
            File.WriteAllText(_localFilePath, movieList); //save the file locally for when theres no wifi
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error downloading movie list: {e.Message}");
            throw;
        }
    }
    
    public async Task<List<Movie>> GetMovieListing()
    {
        await EnsureMovieFileExistsAsync();
        string json = File.ReadAllText(_localFilePath);
        var movies = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
        foreach (var movie in movies)
        {
            // movie.posterUrl = await GetPosterUrlAsync(movie.title, movie.year) ?? "placeholder.png"; // did not work
            movie.posterUrl = await GetPosterUrlAsync(movie.title, movie.year);
            System.Diagnostics.Debug.WriteLine(movie.posterUrl);
        }
        
        return movies;
    }
    
    public async Task EnsureMovieFileExistsAsync()
    {
        if (!File.Exists(_localFilePath))
        {
            await DownloadAndSaveListAsync();
        }
    }
}