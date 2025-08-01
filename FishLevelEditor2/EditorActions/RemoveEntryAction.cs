using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class RemoveEntryAction : EditorAction
    {
        public override string LogMessage => "Remove entry";

        public int EntryIndex { get; set; }
        public LevelEntry EntryToRemove { get; set; }

        public RemoveEntryAction(LevelEntry entryToRemove)
        {
            EntryToRemove = entryToRemove;
        }

        public override void Do(MainViewModel mvm)
        {
            EntryIndex = mvm.LevelViewModel.Level.Entries.IndexOf(EntryToRemove);
            mvm.LevelViewModel.Level.Entries.RemoveAt(EntryIndex);
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.Entries.Insert(EntryIndex, EntryToRemove);
        }
    }
}
