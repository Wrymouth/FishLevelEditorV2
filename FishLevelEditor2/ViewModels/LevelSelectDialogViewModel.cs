using FishLevelEditor2.DataAccess;
using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class LevelSelectDialogViewModel : ViewModelBase
    {
        public WindowManager.NextWindowType NextWindow { get; set; }

        public Project Project { get; set; }

        public LevelSelectDialogViewModel()
        {
            Project = Session.Project;
        }

        public void LoadLevel(int levelIndex)
        {
            Session.Project.MostRecentLevelIndex = levelIndex;
            NextWindow = WindowManager.NextWindowType.Main;
        }
    }
}
