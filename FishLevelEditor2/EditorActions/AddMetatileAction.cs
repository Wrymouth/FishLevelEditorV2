using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class AddMetatileAction : EditorAction
    {
        public override string LogMessage => "Create new metatile";
        public uint PreviouslySelectedMetatileIndex { get; set; }

        public AddMetatileAction(uint previouslySelectedMetatile)
        {
            PreviouslySelectedMetatileIndex = previouslySelectedMetatile;
        }

        public override void Do(MainViewModel mvm)
        {
            mvm.LevelViewModel.Level.MetatileSet.Metatiles.Add(new Metatile("", Metatile.MetatileType.Air, 0, 0, 0, 0));
            mvm.SelectedMetatileViewModel.MetatileIndex = (uint)mvm.LevelViewModel.Level.MetatileSet.Metatiles.Count - 1;

            // size has changed, reset bitmap
            mvm.MetatileSetViewModel.MetatileSetBitmap = new(MetatileSetViewModel.METATILE_SET_IMAGE_WIDTH, MetatileSetViewModel.METATILE_SET_IMAGE_HEIGHT, mvm.LevelViewModel.Level.BackgroundCHR);
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.SelectedMetatileViewModel.MetatileIndex = PreviouslySelectedMetatileIndex;
            mvm.LevelViewModel.Level.MetatileSet.Metatiles.RemoveAt(mvm.LevelViewModel.Level.MetatileSet.Metatiles.Count - 1);
            
            // size has changed, reset bitmap
            mvm.MetatileSetViewModel.MetatileSetBitmap = new(MetatileSetViewModel.METATILE_SET_IMAGE_WIDTH, MetatileSetViewModel.METATILE_SET_IMAGE_HEIGHT, mvm.LevelViewModel.Level.BackgroundCHR);
        }
    }
}
