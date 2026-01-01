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
    
    /**
     *  Gonna put these 4 separate API calls in 1 method ^
     * 
    private async Task<double> GetPopularityAsync(string? title)
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
                return 0;
            }

            var popularity  = results[0].GetProperty("popularity").GetDouble();
            return Math.Round(popularity, 1);
        } catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in GetPopularityAsync: {ex}");
            return 0;
        }
    }
    
    private async Task<string> GetOriginalLanguageAsync(string? title)
    {
        // System.Diagnostics.Debug.WriteLine($"TMDB search: {title}");
        try
        {
            string url = $"{TmdbSearchUrl}?api_key={_tmdbApiKey}&query={Uri.EscapeDataString(title)}";
            string json = await _client.GetStringAsync(url);
            using JsonDocument doc = JsonDocument.Parse(json);
            var results = doc.RootElement.GetProperty("results");
            if (results.GetArrayLength() == 0)
            {
                return "-";
            }

            var originalLanguage = results[0].GetProperty("original_language").GetString();
            if (string.IsNullOrEmpty(originalLanguage))
            {
                return null;
            }

            return originalLanguage;
        } catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error in GetOriginalLanguageAsync: {ex}");
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
            // System.Diagnostics.Debug.WriteLine(json); // debug test
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
            // System.Diagnostics.Debug.WriteLine(json); // debug test
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
    
    **/

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
            
            // movies[i].popularity = await GetPopularityAsync(movies[i].title);
            // movies[i].originalLanguage = await GetOriginalLanguageAsync(movies[i].title);
            // movies[i].posterUrl = await GetPosterUrlAsync(movies[i].title);
            // movies[i].overview = await GetOverviewAsync(movies[i].title);
            var details = await FetchTmdbDetailsAsync(movies[i].title);
            string onlineUrl = $"https://image.tmdb.org/t/p/w500{details.Value.PosterPath}";

            movie.popularity = details.Value.Popularity;
            movie.originalLanguage = details.Value.Language;
            movie.overview = details.Value.Overview;
            movie.posterUrl = onlineUrl;
            
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