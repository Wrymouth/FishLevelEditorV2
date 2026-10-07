using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class DeleteObjectFromLevelAction : EditorAction
    {
        public override string LogMessage => $"Remove {ObjectToDelete.Definition.Name}";

        public LevelObject ObjectToDelete { get; set; }

        public DeleteObjectFromLevelAction(LevelObject objectToDelete)
        {
            ObjectToDelete = objectToDelete;
        }

        public override void Do(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.Objects.Remove(ObjectToDelete);
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.Objects.Add(ObjectToDelete);
        }
    }
}
