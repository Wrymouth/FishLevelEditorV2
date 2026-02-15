using FishLevelEditor2.DataAccess;
using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class OpenProjectDialogViewModel : ViewModelBase
    {
        public WindowManager.NextWindowType NextWindow { get; set; }

        public void CreateProject()
        {
            Project project = new(new ProjectRepository());
            Session.Config.RecentProjectFilePath = "";
            Session.Config.Save();
            OpenProject(project);
        }

        public void LoadProject(string filePath)
        {
            Project project = new ProjectRepository().Load(filePath);
            Session.Config.RecentProjectFilePath = filePath;
            Session.Config.Save();
            OpenProject(project);
        }

        public void OpenProject(Project project)
        {
            Session.Project = project;
            NextWindow = WindowManager.NextWindowType.LevelSelect;
        }
    }
}
