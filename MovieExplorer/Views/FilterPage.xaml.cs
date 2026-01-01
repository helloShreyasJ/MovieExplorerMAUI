using MovieExplorer.ViewModels;

namespace MovieExplorer.Views;

public partial class FilterPage : ContentPage
{
    private readonly MovieViewModel _movieViewModel;
    
    public FilterPage(MovieViewModel movieViewModel)
    {
        InitializeComponent();
        _movieViewModel = movieViewModel;
    }

    // sends the value for the selected filter to the movieViewModel's ApplySort method's parameter and takes user back to homepage
    private async void FilterButton_OnClicked(object sender, EventArgs e)
    {
        var selected = FilterOptionsStack
            .Children
            .OfType<RadioButton>()
            .FirstOrDefault(rb => rb.IsChecked);

        if (selected == null)
            return;

        string filterType = selected.Value.ToString();

        _movieViewModel.ApplySort(filterType);
        await FilterButton.FadeTo(0, 200, Easing.CubicInOut);
        FilterButton.Text = "Applied!";
        await FilterButton.FadeTo(1, 200, Easing.SpringOut);
        await Task.Delay(500);
        FilterButton.Text = "Filter";
        
        await Shell.Current.GoToAsync("..");
    }
}