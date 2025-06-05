using FishLevelEditor2.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.ViewModels
{
    public class NewLevelDialogViewModel
    {
        public enum ModalResult {
            Ok,
            Cancel
        }
        public Level Level { get; set; }

        public ModalResult Result { get; set; }

        public NewLevelDialogViewModel()
        {
            Result = ModalResult.Cancel;
        }

        public void CreateLevel(string name, Level.LevelType levelType, int metatileSetIndex)
        {
            if (name is not null)
            {
                Level = new(name, levelType, metatileSetIndex);
            }
        }
    }
}
