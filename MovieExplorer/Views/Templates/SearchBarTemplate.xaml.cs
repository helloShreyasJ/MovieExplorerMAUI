namespace MovieExplorer.Views.Templates;

public partial class SearchBarTemplate : ContentView
{
    public SearchBarTemplate()
    {
        InitializeComponent();
    }
    private void OnSearchTapped(object sender, EventArgs e)
    {
        SearchEntry.Focus();
    }

    private void SearchEntry_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        SearchLabel.Text = e.NewTextValue;
    }
}