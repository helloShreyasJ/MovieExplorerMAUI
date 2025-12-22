using MovieExplorer.Views;
namespace MovieExplorer;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(MovieDetailPage), typeof(MovieDetailPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(Settings), typeof(Settings));
        Routing.RegisterRoute(nameof(FilterPage), typeof(FilterPage));
    }
}