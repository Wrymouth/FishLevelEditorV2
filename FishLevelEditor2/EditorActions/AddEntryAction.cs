using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class AddEntryAction : EditorAction
    {
        public override string LogMessage => "Add new entry";

        public override void Do(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.Entries.Add(new LevelEntry());
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.Entries.RemoveAt(mvm.LevelViewModel.Level.Entries.Count-1);
        }
    }
}
