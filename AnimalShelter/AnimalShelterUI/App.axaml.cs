using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
namespace AnimalShelterUI;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new AuthorizationWindow();
            // desktop.MainWindow = new MainWindow();
            // desktop.MainWindow = new AnimalWindow();
            // desktop.MainWindow = new GuardianWindow();
            // desktop.MainWindow = new BreedTypeWindow();
            // desktop.MainWindow = new UserWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}