using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FishLevelEditor2.DataAccess;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using FishLevelEditor2.Views;
using System;

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
            if (!(string.IsNullOrWhiteSpace(Session.Config.RecentProjectFilePath)))
            {
                // unimplemented
                throw new NotImplementedException("Loading from file is not implemented yet");
                // load most recent project from disk, load most recent level if available, open main editor with both
            }
            else
            {
                var opdViewModel = new OpenProjectDialogViewModel();
                OpenProjectDialog openProjectDialog = new()
                {
                    DataContext = opdViewModel
                };
                desktop.MainWindow = openProjectDialog;
                openProjectDialog.Show();
                opdViewModel.OpenProjectSuccess += (sender, args) =>
                {
                    Project project = args.Project;
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
