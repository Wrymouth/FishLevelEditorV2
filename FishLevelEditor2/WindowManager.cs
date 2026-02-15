using Avalonia.Controls.ApplicationLifetimes;
using FishLevelEditor2.DataAccess;
using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using FishLevelEditor2.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2
{
    public class WindowManager
    {
        public enum NextWindowType
        {
            None,
            Main,
            OpenProject,
            LevelSelect
        }

        private IClassicDesktopStyleApplicationLifetime desktop;

        public WindowManager(IClassicDesktopStyleApplicationLifetime desktop)
        {
            this.desktop = desktop;
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
                    LaunchMainWindow(Session.Project.MostRecentLevelIndex);
                }
                else
                {
                    LaunchLevelSelectDialog();
                }
            }
            else
            {
                Session.Config.RecentProjectFilePath = "";
                Session.Config.Save();
                LaunchOpenProjectDialog();
            }
        }

        // Handles the main window switching logic
        public void LaunchLevelSelectDialog()
        {
            var lsdvm = new LevelSelectDialogViewModel();
            var levelSelectDialog = new LevelSelectDialog()
            {
                DataContext = lsdvm
            };
            levelSelectDialog.Closed += LevelSelectDialog_Closed;
            desktop.MainWindow = levelSelectDialog;
            levelSelectDialog.Show();
        }

        private void LevelSelectDialog_Closed(object? sender, EventArgs e)
        {
            if (sender is LevelSelectDialog levelSelectDialog)
            {
                if ((levelSelectDialog.DataContext as LevelSelectDialogViewModel).NextWindow == NextWindowType.Main)
                {
                    LaunchMainWindow(Session.Project.MostRecentLevelIndex);
                }
            }
        }

        public void LaunchOpenProjectDialog()
        {
            var opdViewModel = new OpenProjectDialogViewModel();
            var openProjectDialog = new OpenProjectDialog()
            {
                DataContext = opdViewModel
            };
            openProjectDialog.Closed += OpenProjectDialog_Closed;
            desktop.MainWindow = openProjectDialog;
            openProjectDialog.Show();
        }

        private void OpenProjectDialog_Closed(object? sender, EventArgs e)
        {
            if (sender is OpenProjectDialog openProjectDialog)
            {
                if ((openProjectDialog.DataContext as OpenProjectDialogViewModel).NextWindow == NextWindowType.LevelSelect)
                {
                    LaunchLevelSelectDialog();
                }
            }
        }

        public void LaunchMainWindow(int levelIndex)
        {
            var mainWindow = new MainWindow(levelIndex);
            mainWindow.Closed += MainWindow_Closed;
            desktop.MainWindow = mainWindow;
            mainWindow.Show();
        }

        private void MainWindow_Closed(object? sender, EventArgs e)
        {
            if (sender is MainWindow mainWindow)
            {
                if (mainWindow.NextWindow == NextWindowType.LevelSelect)
                {
                    LaunchLevelSelectDialog();
                }
                else if (mainWindow.NextWindow == NextWindowType.OpenProject)
                {
                    LaunchOpenProjectDialog();
                }
            }
        }
    }
}
