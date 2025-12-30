namespace MovieExplorer.Views.Templates;

public partial class SearchBarTemplate : ContentView
{
    public SearchBarTemplate()
    {
        InitializeComponent();
    }
    private async void OnSearchTapped(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//HomePage");
        SearchEntry.Focus();
    }

    private void SearchEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchLabel.Text = e.NewTextValue;
    }
}