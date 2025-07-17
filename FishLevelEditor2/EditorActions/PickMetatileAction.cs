using FishLevelEditor2.Logic;
using FishLevelEditor2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FishLevelEditor2.EditorActions
{
    public class PickMetatileAction : EditorAction
    {
        public uint PreviousSelectedMetatileIndex { get; set; }
        public uint PreviousSelectedPaletteIndex { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public override string LogMessage => "Picked metatile";

        public PickMetatileAction(uint previousSelectedMetatileIndex, uint previousSelectedPaletteIndex, int posX, int posY)
        {
            PreviousSelectedMetatileIndex = previousSelectedMetatileIndex;
            PreviousSelectedPaletteIndex = previousSelectedPaletteIndex;
            PosX = posX;
            PosY = posY;
        }

        public override void Do(MainViewModel mvm)
        {
            ScreenMetatile selectedScreenMetatile = mvm.LevelViewModel.Level.ScreenMetatiles[PosX][PosY];
            mvm.SelectedMetatileViewModel.MetatileIndex = selectedScreenMetatile.mi;
            // TODO palette
        }

        public override void Undo(MainViewModel mvm)
        {
            mvm.SelectedMetatileViewModel.MetatileIndex = PreviousSelectedMetatileIndex;
            // TODO palette
        }
    }
}
