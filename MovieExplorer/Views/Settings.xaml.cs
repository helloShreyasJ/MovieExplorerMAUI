using MovieExplorer.Resources.Styles;

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
}