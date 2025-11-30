using MovieExplorer.ViewModels;

namespace MovieExplorer.Views;

public partial class HomePage : ContentPage
{
    public HomePage(MovieViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm; // getting and setting properties from vm
    }
}