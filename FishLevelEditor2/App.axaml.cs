using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FishLevelEditor2.DataAccess;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using FishLevelEditor2.Views;
using System;
using System.IO;

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
            Session.MasterPalette = new(Session.Config.MasterPaletteFilePath);
            if (File.Exists(Session.Config.RecentProjectFilePath))
            {
                // load most recent project from disk, load most recent level if available, open main editor with both
                Project project = new ProjectRepository().Load(Session.Config.RecentProjectFilePath);
                Session.Project = project;
                if (project.MostRecentLevelIndex >= 0)
                {
                    var mainWindow = new MainWindow(project.MostRecentLevelIndex);
                    desktop.MainWindow = mainWindow;
                    mainWindow.Show();
                }
                else
                {
                    var lsdViewModel = new LevelSelectDialogViewModel();
                    var levelSelectDialog = new LevelSelectDialog()
                    {
                        DataContext = lsdViewModel
                    };
                    levelSelectDialog.Show();
                    lsdViewModel.LoadLevelSuccess += (sender, args) =>
                    {
                        var mainWindow = new MainWindow(args.LevelIndex);
                        desktop.MainWindow = mainWindow;
                        mainWindow.Show();
                        levelSelectDialog.Close();
                    };
                    desktop.MainWindow = levelSelectDialog;

                }
            }
            else
            {
                Session.Config.RecentProjectFilePath = "";
                Session.Config.Save();
                var opdViewModel = new OpenProjectDialogViewModel();
                OpenProjectDialog openProjectDialog = new()
                {
                    DataContext = opdViewModel
                };
                desktop.MainWindow = openProjectDialog;
                openProjectDialog.Show();
                opdViewModel.OpenProjectSuccess += (sender, args) =>
                {
                    var lsdViewModel = new LevelSelectDialogViewModel();
                    var levelSelectDialog = new LevelSelectDialog()
                    {
                        DataContext = lsdViewModel
                    };
                    levelSelectDialog.Show();
                    lsdViewModel.LoadLevelSuccess += (sender, args) =>
                    {
                        var mainWindow = new MainWindow(args.LevelIndex);
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
            throw new NotImplementedException();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
