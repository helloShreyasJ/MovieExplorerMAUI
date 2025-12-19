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
        if (e.Value)
        {
            ChangeTheme();
        }
        else
        {
            ChangeTheme();
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
}