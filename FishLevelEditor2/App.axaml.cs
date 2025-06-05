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
                    DataContext = new MainViewModel(new Level("", Level.LevelType.Horizontal, 0))
                };
            }
            else
            {
                var opdViewModel = new OpenProjectDialogViewModel();
                var mainWindow = new MainWindow();
                OpenProjectDialog openProjectDialog = new()
                {
                    DataContext = opdViewModel
                };
                desktop.MainWindow = openProjectDialog;
                openProjectDialog.Show();
                opdViewModel.OpenProjectSuccess += (sender, args) =>
                {
                    Project project = args.Project;
                    var lsdViewModel = new LevelSelectDialogViewModel(project);
                    var levelSelectDialog = new LevelSelectDialog()
                    {
                        DataContext = lsdViewModel
                    };
                    levelSelectDialog.Show();
                    lsdViewModel.LoadLevelSuccess += (sender, args) =>
                    {
                        int levelIndex = args.LevelIndex;
                        mainWindow.DataContext = new MainViewModel(Session.Project.Levels[levelIndex]);
                        desktop.MainWindow = mainWindow;
                        mainWindow.Show();
                        levelSelectDialog.Close();
                    };
                    openProjectDialog.Close();
                    desktop.MainWindow = levelSelectDialog;

                };
            }
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = new MainView
            {
                DataContext = new MainViewModel(new Level("", Level.LevelType.Horizontal, 0))
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
