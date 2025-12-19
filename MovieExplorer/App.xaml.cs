using MovieExplorer.Resources.Styles;

namespace MovieExplorer;

public partial class App : Application
{
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
        Application.Current.Resources.MergedDictionaries.Add(new LightTheme());
        Application.Current.UserAppTheme = AppTheme.Light;
    }
}