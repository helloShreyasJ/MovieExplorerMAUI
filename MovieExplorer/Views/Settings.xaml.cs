using MovieExplorer.Resources.Styles;
using MovieExplorer.ViewModels;

namespace MovieExplorer.Views;

public partial class Settings : ContentPage
{
    private static readonly LightTheme LightTheme = new LightTheme();
    private static readonly DarkTheme DarkTheme =  new DarkTheme();
    
    public Settings()
    {
        InitializeComponent();
    }

    private void DarkMode_OnToggled(object? sender, ToggledEventArgs e)
    {
        Preferences.Set("DarkModeEnabled", e.Value);
        
        if (e.Value)
        {
            ChangeTheme();
            App.SetLogo("logo_dark.png");
        }
        else
        {
            ChangeTheme();
            App.SetLogo("logo_light.png");
        }
    }

    public static AppTheme ChangeTheme()
    {
        ICollection<ResourceDictionary> mergedDictionaries = Application.Current.Resources.MergedDictionaries;
        if (Application.Current.UserAppTheme == AppTheme.Dark)
        {
            Application.Current.UserAppTheme = AppTheme.Light;
            mergedDictionaries.Remove(DarkTheme);
            mergedDictionaries.Add(LightTheme);
        }
        else
        {
            Application.Current.UserAppTheme = AppTheme.Dark;
            mergedDictionaries.Remove(LightTheme);
            mergedDictionaries.Add(DarkTheme);
            
        }
        
        return Application.Current.UserAppTheme;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();

        bool isDark = Preferences.Get("DarkModeEnabled", false);

        DarkModeSwitch.Toggled -= DarkMode_OnToggled;
        DarkModeSwitch.IsToggled = isDark;
        DarkModeSwitch.Toggled += DarkMode_OnToggled;
    }

    private async void AddKeyButton_OnClicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TMDBApiKey.Text))
        {
            DisplayAlert("MovieExplorer", "No Input Entered.", "OK");
            return;
        }
        
        await AddKeyButton.FadeTo(0, 200, Easing.CubicInOut);
        AddKeyButton.ImageSource = "tick.png";
        await AddKeyButton.FadeTo(1, 200, Easing.CubicInOut);
        await Task.Delay(400);
        AddKeyButton.ImageSource = "add.png";
        
        DisplayAlert("MovieExplorer", "API Key Applied. Reload Page.", "OK");
        Key.personalKey = TMDBApiKey.Text;
        Preferences.Set("ApiKey", Key.personalKey);
        System.Diagnostics.Debug.WriteLine($"API Key updated: {Key.personalKey}");
        await Shell.Current.GoToAsync("..");
    }

    private async void BackButton_OnClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//HomePage");
    }

    private async void HyperLink_OnTapped(object? sender, TappedEventArgs e)
    {
        await Launcher.OpenAsync("https://www.themoviedb.org/settings/api");
        HyperlinkToApi.TextColor = Colors.MediumSlateBlue;
    }

    private async void TMDBEntryApiKey_OnFocused(object? sender, FocusEventArgs e)
    {
        HyperlinkToApi.Opacity = 0;
        HyperlinkToApi.IsVisible = true;
        await HyperlinkToApi.FadeTo(1, 400, Easing.Linear);
    }

    private async void TMDBEntryApiKey_OnUnfocused(object? sender, FocusEventArgs e)
    {
        await HyperlinkToApi.FadeTo(0, 400, Easing.Linear);
        HyperlinkToApi.IsVisible = false;
    }
}