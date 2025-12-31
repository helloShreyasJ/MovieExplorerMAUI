namespace MovieExplorer.Models;

public class Movie
{
    public string title { get; set; }
    public int year { get; set; }
    public List<string> genre { get; set; }
    public string director { get; set; }
    public double rating { get; set; }
    public string emoji { get; set; }
    public string posterUrl { get; set; }
    public string overview { get; set; }
    public string originalLanguage { get; set; }
    public string popularity { get; set; }
    
    public string GenreText => string.Join(", ", genre);
}