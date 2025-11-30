namespace MovieExplorer.Models;

public class Movie
{
    public string title { get; set; }
    public int year { get; set; }
    public List<string> genre { get; set; }
    public string director { get; set; }
    public double rating { get; set; }
    public string emoji { get; set; }
    
    public string GenreText => string.Join(", ", genre);
}