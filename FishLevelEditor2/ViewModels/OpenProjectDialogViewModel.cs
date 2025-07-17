using FishLevelEditor2.DataAccess;
using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public delegate void OpenProjectEventHandler(object sender, OpenProjectEventArgs e);
    public class OpenProjectDialogViewModel : ViewModelBase
    {
        public event OpenProjectEventHandler OpenProjectSuccess;

        public void CreateProject()
        {
            Project project = new(new ProjectRepository());
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
            OpenProjectSuccess?.Invoke(
              this,
              new OpenProjectEventArgs(project)
            );
        }
    }
}
