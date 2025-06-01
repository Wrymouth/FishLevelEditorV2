using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FishLevelEditor2.DataAccess;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using FishLevelEditor2.Views;

namespace FishLevelEditor2;

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
            // check if a project already exists in the config
            Session.Config = new(new ConfigRepository());
            if (!(string.IsNullOrWhiteSpace(Session.Config.RecentProjectFilePath)))
            {
                // unimplemented
                // load most recent project from disk, load most recent level if available, open main editor with both
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainViewModel()
                };
            }
            else
            {
                var opdViewModel = new OpenProjectDialogViewModel();
                var mainWindowViewModel = new MainViewModel();
                OpenProjectDialog openProjectDialog = new()
                {
                    DataContext = opdViewModel
                };
                desktop.MainWindow = openProjectDialog;
                openProjectDialog.Show();
                opdViewModel.OpenProjectSuccess += (sender, args) =>
                {
                    var mainWindow = new MainWindow()
                    {
                        DataContext = mainWindowViewModel
                    };
                    mainWindow.Show();
                    openProjectDialog.Close();
                    desktop.MainWindow = mainWindow;
                };
            }
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
