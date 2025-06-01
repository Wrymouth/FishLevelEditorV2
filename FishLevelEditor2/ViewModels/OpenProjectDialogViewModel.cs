using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public delegate void OpenProjectEventHandler(object sender, EventArgs e);
    public class OpenProjectDialogViewModel : ViewModelBase
    {
        public event OpenProjectEventHandler OpenProjectSuccess;

        public void CreateProject()
        {
            Project project = new();
            OpenProjectSuccess?.Invoke(
                this,
                new OpenProjectEventArgs(project)
            );
        }

        public void LoadProject()
        {
            Project project = new();
            OpenProjectSuccess?.Invoke(
              this,
              new OpenProjectEventArgs(project)
            );
        }
    }
}
