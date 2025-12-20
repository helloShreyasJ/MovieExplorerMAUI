using MovieExplorer.Resources.Styles;

namespace MovieExplorer;

public partial class App : Application
{
    private static readonly LightTheme LightTheme = new LightTheme();
    private static readonly DarkTheme DarkTheme = new DarkTheme();
    
    public App()
    {
        InitializeComponent();
        SetInitialTheme();
    }
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
    
    private void SetInitialTheme()
    {
        // gets prefs, makes changes into merged dictionary, sets current app theme
        bool isDark = Preferences.Get("DarkModeEnabled", false);
        var mergedDict = Application.Current.Resources.MergedDictionaries;
        if (isDark)
        {
            // mergedDict.Remove(new LightTheme());
            // mergedDict.Add(new DarkTheme());
            mergedDict.Remove(LightTheme);
            mergedDict.Add(DarkTheme);
            Application.Current.UserAppTheme = AppTheme.Dark;
            SetLogo("logo_dark.png");
        }
        else
        {
            // mergedDict.Remove(new DarkTheme());
            // mergedDict.Add(new LightTheme());
            mergedDict.Remove(DarkTheme);
            mergedDict.Add(LightTheme);
            Application.Current.UserAppTheme = AppTheme.Light;
            SetLogo("logo_light.png");
        }
    }
    
    public static void SetLogo(string logo)
    {
        Application.Current.Resources["AppLogo"] = logo;
    }
}