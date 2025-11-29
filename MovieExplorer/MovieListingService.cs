using System.Text.Json;
using Path = System.IO.Path;
namespace MovieExplorer;

public class MovieListingService
{
    private const string MovieListUrl = "https://raw.githubusercontent.com/DonH-ITS/jsonfiles/refs/heads/main/moviesemoji.json";
    private readonly string _localFilePath;
    // using only one HttpClient
    private readonly HttpClient _client;

    public MovieListingService()
    {
        string localFolder = FileSystem.AppDataDirectory; // local path for getting list
        _localFilePath = Path.Combine(localFolder, "moviesemoji.json");
        _client = new HttpClient();
    }

    private async Task DownloadAndSaveListAsync()
    {
        try
        {
            string movieList = await _client.GetStringAsync(MovieListUrl);
            File.WriteAllText(_localFilePath, movieList); //save the file locally
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error downloading movie list: {e.Message}");
            throw;
        }
    }
    
    public async Task<List<MovieListing>> GetMovieListing()
    {
        await EnsureMovieFileExistsAsync();
        string json = File.ReadAllText(_localFilePath);
        System.Diagnostics.Debug.WriteLine(json);
        return JsonSerializer.Deserialize<List<MovieListing>>(json) ?? new List<MovieListing>();
    }
    
    public async Task EnsureMovieFileExistsAsync()
    {
        if (!File.Exists(_localFilePath))
        {
            await DownloadAndSaveListAsync();
        }
    }
}