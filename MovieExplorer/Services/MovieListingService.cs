using System.Text.Json;
using MovieExplorer.Models;
using Path = System.IO.Path;
namespace MovieExplorer.Services;

public class MovieListingService
{
    private const string MovieListUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";
    private readonly string _localFilePath;
    // private string _tmdbApiKey = Key.personalKey; // IMPORTANT: tmdb api key needs to be here to get the images
    private string _tmdbApiKey => Key.personalKey; // get latest value of key
    private const string TmdbSearchUrl = "https://api.themoviedb.org/3/search/movie";
    private readonly HttpClient _client; // using only one HttpClient

    public MovieListingService()
    {
        string localFolder = FileSystem.AppDataDirectory; // local path for getting list
        _localFilePath = Path.Combine(localFolder, "moviesemoji.json");
        _client = new HttpClient();
    }
    
    private async Task<string> GetOriginalLanguageAsync(string? title)
    {
        System.Diagnostics.Debug.WriteLine($"TMDB search: {title}");
        try
        {
            string url = $"{TmdbSearchUrl}?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(title)}";
            string json = await _client.GetStringAsync(url);
            System.Diagnostics.Debug.WriteLine(json); // debug test
            using JsonDocument doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");
            if (results.GetArrayLength() == 0)
            {
                return "-";
            }

            var overview = results[0].GetProperty("original_language").GetString();
            if (string.IsNullOrEmpty(overview))
            {
                return null;
            }

            return overview;
        } catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in GetOverviewAsync: {ex}");
            return "-";
        }
    }
    
    private async Task<string> GetOverviewAsync(string? title)
    {
        // System.Diagnostics.Debug.WriteLine($"TMDB search: {title}");
        try
        {
            string url = $"{TmdbSearchUrl}?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(title)}";
            string json = await _client.GetStringAsync(url);
            System.Diagnostics.Debug.WriteLine(json); // debug test
            using JsonDocument doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");
            if (results.GetArrayLength() == 0)
            {
                return "No overview found for this movie.";
            }

            var overview = results[0].GetProperty("overview").GetString();
            if (string.IsNullOrEmpty(overview))
            {
                return null;
            }

            return overview;
        } catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in GetOverviewAsync: {ex}");
            return "Overview unavailable. API Key Required.";
        }
    }

    private async Task<string> GetPosterUrlAsync(string? title)
    {
        // System.Diagnostics.Debug.WriteLine($"TMDB search: {title}");
        try
        {
            string url =  $"{TmdbSearchUrl}?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(title)}";
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
            movie.originalLanguage = await GetOriginalLanguageAsync(movie.title);
            movie.posterUrl = await GetPosterUrlAsync(movie.title);
            movie.overview = await GetOverviewAsync(movie.title);
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