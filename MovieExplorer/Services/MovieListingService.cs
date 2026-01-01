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
    
    private async Task<(double Popularity, string Language, string Overview, string PosterPath)?> FetchTmdbDetailsAsync(string? title)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(title)) return null;

            string url = $"{TmdbSearchUrl}?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(title)}";
            string json = await _client.GetStringAsync(url);
            
            using JsonDocument doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");

            if (results.GetArrayLength() == 0) return null;

            var firstResult = results[0];

            double popularity = firstResult.TryGetProperty("popularity", out var p) ? p.GetDouble() : 0;
            string language = firstResult.TryGetProperty("original_language", out var l) ? l.GetString() ?? "-" : "-";
            string overview = firstResult.TryGetProperty("overview", out var o) ? o.GetString() ?? "No overview" : "No overview";
            string poster = firstResult.TryGetProperty("poster_path", out var pp) ? pp.GetString() ?? "" : "";

            return (Math.Round(popularity, 1), language, overview, poster);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"API Error for {title}: {ex.Message}");
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
    
    public async Task<List<Movie>> GetMovieListing(IProgress<double>? progress = null)
    {
        await EnsureMovieFileExistsAsync();
        string json = File.ReadAllText(_localFilePath);
        var movies = JsonSerializer.Deserialize<List<Movie>>(json) ?? new List<Movie>();
        
        for(int i = 0; i < movies.Count; i++)
        {
            var movie = movies[i];
            var details = await FetchTmdbDetailsAsync(movies[i].title);
            // string onlineUrl = $"https://image.tmdb.org/t/p/w500{details.Value.PosterPath}";

            
            if (details.HasValue)
            {
                movie.popularity = details.Value.Popularity;
                movie.originalLanguage = details.Value.Language;
                movie.overview = details.Value.Overview;
                string onlineUrl = $"https://image.tmdb.org/t/p/w500{details.Value.PosterPath}";
                movie.posterUrl = onlineUrl;
            }
            else
            {
                movie.popularity = 0;
                movie.originalLanguage = "-";
                movie.overview = "No overview available (API key required)";
                movie.posterUrl = null;
            }

            System.Diagnostics.Debug.WriteLine(movies[i].posterUrl);
            progress?.Report((i + 1) / (double)movies.Count);
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