using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public delegate void LoadLevelEventHandler(object sender, LoadLevelEventArgs e);
    public class LevelSelectDialogViewModel : ViewModelBase
    {
        public event LoadLevelEventHandler LoadLevelSuccess;
        public Project Project { get; set; }
        public LevelSelectDialogViewModel(Project project)
        {
            Project = project;
        }

        public void LoadLevel(int levelIndex)
        {
            LoadLevelSuccess?.Invoke(
                this,
                new LoadLevelEventArgs(levelIndex)
            );
        }
    }
}
