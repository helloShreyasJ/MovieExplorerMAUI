using System.Diagnostics;
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
        SetAPIKey();
    }
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        const int newHeight = 1000;
        const int newWidth = 800;

        var newWindow = new Window(new AppShell())
        {
            MinimumHeight = newHeight,
            MinimumWidth = newWidth
        };

        return newWindow;
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

    private static void SetAPIKey()
    {
        Key.personalKey = Preferences.Get("ApiKey", Key.personalKey);
        Debug.WriteLine($"API Key loaded on startup: {Key.personalKey}");
    }
}